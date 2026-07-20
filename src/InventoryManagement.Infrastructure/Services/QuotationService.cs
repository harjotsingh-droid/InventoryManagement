using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Services;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Services;

public class QuotationService : IQuotationService
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;
    private readonly ICompanySettingsService _settingsService;

    public QuotationService(
        ApplicationDbContext db,
        ICurrentUserService currentUser,
        ICompanySettingsService settingsService)
    {
        _db = db;
        _currentUser = currentUser;
        _settingsService = settingsService;
    }

    public async Task<ServiceResult<IReadOnlyList<QuotationListItemDto>>> GetAllAsync()
    {
        var items = await _db.Quotations
            .AsNoTracking()
            .Where(q => q.CompanyId == _currentUser.CompanyId)
            .Include(q => q.Customer)
            .OrderByDescending(q => q.QuotationDate)
            .ThenByDescending(q => q.Id)
            .Select(q => new QuotationListItemDto
            {
                Id = q.Id,
                QuotationNumber = q.QuotationNumber,
                CustomerName = q.Customer.Name,
                QuotationDate = q.QuotationDate,
                TotalAmount = q.TotalAmount
            })
            .ToListAsync();

        return ServiceResult<IReadOnlyList<QuotationListItemDto>>.Ok(items);
    }

    public async Task<ServiceResult<QuotationDto>> GetByIdAsync(int id)
    {
        var quotation = await _db.Quotations
            .AsNoTracking()
            .Include(q => q.Customer)
            .Include(q => q.Lines)
            .ThenInclude(l => l.Product)
            .FirstOrDefaultAsync(q => q.Id == id && q.CompanyId == _currentUser.CompanyId);

        return quotation == null
            ? ServiceResult<QuotationDto>.Fail("Quotation not found.")
            : ServiceResult<QuotationDto>.Ok(Map(quotation));
    }

    public async Task<ServiceResult<QuotationDto>> CreateAsync(CreateQuotationDto dto)
    {
        var validation = ValidateCreate(dto);
        if (validation != null)
        {
            return validation;
        }

        var customer = await _db.Customers.FirstOrDefaultAsync(c =>
            c.Id == dto.CustomerId && c.CompanyId == _currentUser.CompanyId);

        if (customer == null)
        {
            return ServiceResult<QuotationDto>.Fail("Selected customer was not found.");
        }

        var productIds = dto.Lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await _db.Products
            .Where(p => p.CompanyId == _currentUser.CompanyId && productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        if (products.Count != productIds.Count)
        {
            return ServiceResult<QuotationDto>.Fail("One or more selected products were not found.");
        }

        var lineInputs = dto.Lines.Select(l =>
            (l.Quantity, l.UnitPrice, l.DiscountPercent, l.GstPercent)).ToList();

        var calculation = QuotationCalculator.Calculate(lineInputs, dto.DiscountAmount);

        var quotationNumber = await GenerateQuotationNumberAsync();

        var quotation = new Quotation
        {
            CompanyId = _currentUser.CompanyId,
            QuotationNumber = quotationNumber,
            CustomerId = dto.CustomerId,
            QuotationDate = dto.QuotationDate.Date,
            ValidUntil = dto.ValidUntil.Date,
            SubTotal = calculation.SubTotal,
            TaxAmount = calculation.TaxAmount,
            DiscountAmount = dto.DiscountAmount,
            TotalAmount = calculation.TotalAmount,
            Notes = dto.Notes?.Trim()
        };

        for (var i = 0; i < dto.Lines.Count; i++)
        {
            var input = dto.Lines[i];
            var lineCalc = calculation.Lines[i];
            var product = products[input.ProductId];

            quotation.Lines.Add(new QuotationLine
            {
                ProductId = product.Id,
                Quantity = input.Quantity,
                UnitPrice = input.UnitPrice,
                DiscountPercent = input.DiscountPercent,
                GstPercent = input.GstPercent,
                TaxAmount = lineCalc.TaxAmount,
                TotalAmount = lineCalc.TotalAmount
            });
        }

        _db.Quotations.Add(quotation);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(quotation.Id);
    }

    public async Task<ServiceResult<QuotationPdfContextDto>> GetPdfContextAsync(int id)
    {
        var quotationResult = await GetByIdAsync(id);
        if (!quotationResult.Success || quotationResult.Data == null)
        {
            return ServiceResult<QuotationPdfContextDto>.Fail(quotationResult.ErrorMessage ?? "Quotation not found.");
        }

        var settingsResult = await _settingsService.GetSettingsAsync();
        if (!settingsResult.Success || settingsResult.Data == null)
        {
            return ServiceResult<QuotationPdfContextDto>.Fail(settingsResult.ErrorMessage ?? "Settings not found.");
        }

        return ServiceResult<QuotationPdfContextDto>.Ok(new QuotationPdfContextDto
        {
            Quotation = quotationResult.Data,
            Settings = settingsResult.Data
        });
    }

    private async Task<string> GenerateQuotationNumberAsync()
    {
        var year = DateTime.Today.Year;
        var prefix = $"QT-{year}-";

        var lastNumber = await _db.Quotations
            .Where(q => q.CompanyId == _currentUser.CompanyId && q.QuotationNumber.StartsWith(prefix))
            .OrderByDescending(q => q.QuotationNumber)
            .Select(q => q.QuotationNumber)
            .FirstOrDefaultAsync();

        var next = 1;
        if (!string.IsNullOrEmpty(lastNumber))
        {
            var suffix = lastNumber[prefix.Length..];
            if (int.TryParse(suffix, out var parsed))
            {
                next = parsed + 1;
            }
        }

        return $"{prefix}{next:D4}";
    }

    private static ServiceResult<QuotationDto>? ValidateCreate(CreateQuotationDto dto)
    {
        var inErrors = new Dictionary<string, string[]>();

        if (dto.CustomerId <= 0)
        {
            inErrors["CustomerId"] = ["Customer is required."];
        }

        if (dto.ValidUntil.Date < dto.QuotationDate.Date)
        {
            inErrors["ValidUntil"] = ["Valid until date cannot be before quotation date."];
        }

        if (dto.Lines == null || dto.Lines.Count == 0)
        {
            inErrors["Lines"] = ["At least one line item is required."];
        }
        else
        {
            for (var i = 0; i < dto.Lines.Count; i++)
            {
                var line = dto.Lines[i];
                if (line.ProductId <= 0)
                {
                    inErrors[$"Lines[{i}].ProductId"] = ["Product is required."];
                }

                if (line.Quantity <= 0)
                {
                    inErrors[$"Lines[{i}].Quantity"] = ["Quantity must be greater than zero."];
                }

                if (line.UnitPrice < 0)
                {
                    inErrors[$"Lines[{i}].UnitPrice"] = ["Unit price cannot be negative."];
                }

                if (line.DiscountPercent < 0 || line.DiscountPercent > 100)
                {
                    inErrors[$"Lines[{i}].DiscountPercent"] = ["Discount must be between 0 and 100."];
                }

                if (line.GstPercent < 0 || line.GstPercent > 100)
                {
                    inErrors[$"Lines[{i}].GstPercent"] = ["GST must be between 0 and 100."];
                }
            }
        }

        if (dto.DiscountAmount < 0)
        {
            inErrors["DiscountAmount"] = ["Quotation discount cannot be negative."];
        }

        return inErrors.Count > 0
            ? ServiceResult<QuotationDto>.ValidationFail(inErrors)
            : null;
    }

    private static QuotationDto Map(Quotation q) => new()
    {
        Id = q.Id,
        QuotationNumber = q.QuotationNumber,
        CustomerId = q.CustomerId,
        CustomerName = q.Customer.Name,
        QuotationDate = q.QuotationDate,
        ValidUntil = q.ValidUntil,
        SubTotal = q.SubTotal,
        TaxAmount = q.TaxAmount,
        DiscountAmount = q.DiscountAmount,
        TotalAmount = q.TotalAmount,
        Notes = q.Notes,
        Lines = q.Lines.Select(l => new QuotationLineDto
        {
            Id = l.Id,
            ProductId = l.ProductId,
            ProductName = l.Product.Name,
            ProductSku = l.Product.Sku,
            Quantity = l.Quantity,
            UnitPrice = l.UnitPrice,
            DiscountPercent = l.DiscountPercent,
            GstPercent = l.GstPercent,
            TaxAmount = l.TaxAmount,
            TotalAmount = l.TotalAmount
        }).ToList()
    };
}
