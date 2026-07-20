using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface ISearchService
{
    Task<ServiceResult<GlobalSearchResultDto>> SearchAsync(string query);
}
