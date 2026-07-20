using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface ICustomerService
{
    Task<ServiceResult<IReadOnlyList<CustomerDto>>> GetAllAsync(string? search = null);
    Task<ServiceResult<CustomerDto>> GetByIdAsync(int id);
    Task<ServiceResult<CustomerDto>> CreateAsync(CreateCustomerDto dto);
    Task<ServiceResult<CustomerDto>> UpdateAsync(UpdateCustomerDto dto);
}
