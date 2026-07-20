using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DashboardService(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ServiceResult<DashboardDto>> GetDashboardAsync()
    {
        var companyId = _currentUser.CompanyId;
        var today = DateTime.Today;

        var productCount = await _db.Products.CountAsync(p => p.CompanyId == companyId);
        var customerCount = await _db.Customers.CountAsync(c => c.CompanyId == companyId);
        var todaysQuotationCount = await _db.Quotations.CountAsync(q =>
            q.CompanyId == companyId && q.QuotationDate == today);
        var pendingQuotationCount = await _db.Quotations.CountAsync(q =>
            q.CompanyId == companyId && q.ValidUntil >= today);

        return ServiceResult<DashboardDto>.Ok(new DashboardDto
        {
            ProductCount = productCount,
            CustomerCount = customerCount,
            TodaysQuotationCount = todaysQuotationCount,
            PendingQuotationCount = pendingQuotationCount
        });
    }
}
