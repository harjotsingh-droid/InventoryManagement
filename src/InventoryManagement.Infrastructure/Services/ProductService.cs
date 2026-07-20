using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ProductService(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ServiceResult<IReadOnlyList<ProductDto>>> GetAllAsync(string? search = null)
    {
        var query = _db.Products
            .AsNoTracking()
            .Where(p => p.CompanyId == _currentUser.CompanyId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p =>
                p.Name.Contains(term) ||
                p.Sku.Contains(term) ||
                (p.Barcode != null && p.Barcode.Contains(term)));
        }

        var items = await query
            .OrderBy(p => p.Name)
            .Select(p => Map(p))
            .ToListAsync();

        return ServiceResult<IReadOnlyList<ProductDto>>.Ok(items);
    }

    public async Task<ServiceResult<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.CompanyId == _currentUser.CompanyId);

        return product == null
            ? ServiceResult<ProductDto>.Fail("Product not found.")
            : ServiceResult<ProductDto>.Ok(Map(product));
    }

    public async Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        var validation = Validate(dto.Name, dto.Sku, dto.SellingPrice, dto.GstPercent, dto.CurrentStock);
        if (validation != null)
        {
            return validation;
        }

        var exists = await _db.Products.AnyAsync(p =>
            p.CompanyId == _currentUser.CompanyId && p.Sku == dto.Sku.Trim());

        if (exists)
        {
            return ServiceResult<ProductDto>.Fail("A product with this SKU already exists.");
        }

        var product = new Product
        {
            CompanyId = _currentUser.CompanyId,
            Name = dto.Name.Trim(),
            Sku = dto.Sku.Trim(),
            Barcode = dto.Barcode?.Trim(),
            SellingPrice = dto.SellingPrice,
            GstPercent = dto.GstPercent,
            CurrentStock = dto.CurrentStock
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return ServiceResult<ProductDto>.Ok(Map(product));
    }

    public async Task<ServiceResult<ProductDto>> UpdateAsync(UpdateProductDto dto)
    {
        var validation = Validate(dto.Name, dto.Sku, dto.SellingPrice, dto.GstPercent, dto.CurrentStock);
        if (validation != null)
        {
            return validation;
        }

        var product = await _db.Products.FirstOrDefaultAsync(p =>
            p.Id == dto.Id && p.CompanyId == _currentUser.CompanyId);

        if (product == null)
        {
            return ServiceResult<ProductDto>.Fail("Product not found.");
        }

        var skuExists = await _db.Products.AnyAsync(p =>
            p.CompanyId == _currentUser.CompanyId &&
            p.Sku == dto.Sku.Trim() &&
            p.Id != dto.Id);

        if (skuExists)
        {
            return ServiceResult<ProductDto>.Fail("A product with this SKU already exists.");
        }

        product.Name = dto.Name.Trim();
        product.Sku = dto.Sku.Trim();
        product.Barcode = dto.Barcode?.Trim();
        product.SellingPrice = dto.SellingPrice;
        product.GstPercent = dto.GstPercent;
        product.CurrentStock = dto.CurrentStock;

        await _db.SaveChangesAsync();
        return ServiceResult<ProductDto>.Ok(Map(product));
    }

    private static ServiceResult<ProductDto>? Validate(
        string name, string sku, decimal sellingPrice, decimal gstPercent, int currentStock)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors["Name"] = ["Product name is required."];
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            errors["Sku"] = ["SKU is required."];
        }

        if (sellingPrice < 0)
        {
            errors["SellingPrice"] = ["Selling price cannot be negative."];
        }

        if (gstPercent < 0 || gstPercent > 100)
        {
            errors["GstPercent"] = ["GST must be between 0 and 100."];
        }

        if (currentStock < 0)
        {
            errors["CurrentStock"] = ["Stock cannot be negative."];
        }

        return errors.Count > 0
            ? ServiceResult<ProductDto>.ValidationFail(errors)
            : null;
    }

    private static ProductDto Map(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Sku = p.Sku,
        Barcode = p.Barcode,
        SellingPrice = p.SellingPrice,
        GstPercent = p.GstPercent,
        CurrentStock = p.CurrentStock
    };
}
