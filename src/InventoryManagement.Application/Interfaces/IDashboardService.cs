using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface IDashboardService
{
    Task<ServiceResult<DashboardDto>> GetDashboardAsync();
}
