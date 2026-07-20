using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface ICompanySettingsService
{
    Task<ServiceResult<CompanySettingsDto>> GetSettingsAsync();
    Task<ServiceResult<CompanySettingsDto>> UpdateSettingsAsync(UpdateCompanySettingsDto dto);
}
