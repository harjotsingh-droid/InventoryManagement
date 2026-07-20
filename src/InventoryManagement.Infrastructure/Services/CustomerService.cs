using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CustomerService(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ServiceResult<IReadOnlyList<CustomerDto>>> GetAllAsync(string? search = null)
    {
        var query = _db.Customers
            .AsNoTracking()
            .Where(c => c.CompanyId == _currentUser.CompanyId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                c.Name.Contains(term) ||
                c.Code.Contains(term) ||
                (c.Mobile != null && c.Mobile.Contains(term)) ||
                (c.City != null && c.City.Contains(term)));
        }

        var items = await query
            .OrderBy(c => c.Name)
            .Select(c => Map(c))
            .ToListAsync();

        return ServiceResult<IReadOnlyList<CustomerDto>>.Ok(items);
    }

    public async Task<ServiceResult<CustomerDto>> GetByIdAsync(int id)
    {
        var customer = await _db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == _currentUser.CompanyId);

        return customer == null
            ? ServiceResult<CustomerDto>.Fail("Customer not found.")
            : ServiceResult<CustomerDto>.Ok(Map(customer));
    }

    public async Task<ServiceResult<CustomerDto>> CreateAsync(CreateCustomerDto dto)
    {
        var validation = Validate(dto.Name, dto.Code);
        if (validation != null)
        {
            return validation;
        }

        var exists = await _db.Customers.AnyAsync(c =>
            c.CompanyId == _currentUser.CompanyId && c.Code == dto.Code.Trim());

        if (exists)
        {
            return ServiceResult<CustomerDto>.Fail("A customer with this code already exists.");
        }

        var customer = new Customer
        {
            CompanyId = _currentUser.CompanyId,
            Name = dto.Name.Trim(),
            Code = dto.Code.Trim(),
            Mobile = dto.Mobile?.Trim(),
            City = dto.City?.Trim(),
            State = dto.State?.Trim(),
            Address = dto.Address?.Trim()
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        return ServiceResult<CustomerDto>.Ok(Map(customer));
    }

    public async Task<ServiceResult<CustomerDto>> UpdateAsync(UpdateCustomerDto dto)
    {
        var validation = Validate(dto.Name, dto.Code);
        if (validation != null)
        {
            return validation;
        }

        var customer = await _db.Customers.FirstOrDefaultAsync(c =>
            c.Id == dto.Id && c.CompanyId == _currentUser.CompanyId);

        if (customer == null)
        {
            return ServiceResult<CustomerDto>.Fail("Customer not found.");
        }

        var codeExists = await _db.Customers.AnyAsync(c =>
            c.CompanyId == _currentUser.CompanyId &&
            c.Code == dto.Code.Trim() &&
            c.Id != dto.Id);

        if (codeExists)
        {
            return ServiceResult<CustomerDto>.Fail("A customer with this code already exists.");
        }

        customer.Name = dto.Name.Trim();
        customer.Code = dto.Code.Trim();
        customer.Mobile = dto.Mobile?.Trim();
        customer.City = dto.City?.Trim();
        customer.State = dto.State?.Trim();
        customer.Address = dto.Address?.Trim();

        await _db.SaveChangesAsync();
        return ServiceResult<CustomerDto>.Ok(Map(customer));
    }

    private static ServiceResult<CustomerDto>? Validate(string name, string code)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors["Name"] = ["Customer name is required."];
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            errors["Code"] = ["Customer code is required."];
        }

        return errors.Count > 0
            ? ServiceResult<CustomerDto>.ValidationFail(errors)
            : null;
    }

    private static CustomerDto Map(Customer c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Code = c.Code,
        Mobile = c.Mobile,
        City = c.City,
        State = c.State,
        Address = c.Address
    };
}
