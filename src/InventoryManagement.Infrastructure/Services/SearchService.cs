using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public SearchService(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ServiceResult<GlobalSearchResultDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
        {
            return ServiceResult<GlobalSearchResultDto>.Fail("Enter at least 2 characters to search.");
        }

        var term = query.Trim();
        var companyId = _currentUser.CompanyId;

        var products = await _db.Products
            .AsNoTracking()
            .Where(p => p.CompanyId == companyId &&
                        (p.Name.Contains(term) || p.Sku.Contains(term) ||
                         (p.Barcode != null && p.Barcode.Contains(term))))
            .OrderBy(p => p.Name)
            .Take(10)
            .Select(p => new SearchItemDto
            {
                Title = p.Name,
                Subtitle = $"SKU: {p.Sku} | Stock: {p.CurrentStock}",
                Url = $"/Products/Edit/{p.Id}"
            })
            .ToListAsync();

        var customers = await _db.Customers
            .AsNoTracking()
            .Where(c => c.CompanyId == companyId &&
                        (c.Name.Contains(term) || c.Code.Contains(term) ||
                         (c.Mobile != null && c.Mobile.Contains(term))))
            .OrderBy(c => c.Name)
            .Take(10)
            .Select(c => new SearchItemDto
            {
                Title = c.Name,
                Subtitle = $"Code: {c.Code} | {c.City}",
                Url = $"/Customers/Edit/{c.Id}"
            })
            .ToListAsync();

        var quotations = await _db.Quotations
            .AsNoTracking()
            .Where(q => q.CompanyId == companyId &&
                        (q.QuotationNumber.Contains(term) || q.Customer.Name.Contains(term)))
            .Include(q => q.Customer)
            .OrderByDescending(q => q.QuotationDate)
            .Take(10)
            .Select(q => new SearchItemDto
            {
                Title = q.QuotationNumber,
                Subtitle = $"{q.Customer.Name} | {q.TotalAmount:C}",
                Url = $"/Quotations/Details/{q.Id}"
            })
            .ToListAsync();

        return ServiceResult<GlobalSearchResultDto>.Ok(new GlobalSearchResultDto
        {
            Products = products,
            Customers = customers,
            Quotations = quotations
        });
    }
}
