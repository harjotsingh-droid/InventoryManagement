using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface IProductService
{
    Task<ServiceResult<IReadOnlyList<ProductDto>>> GetAllAsync(string? search = null);
    Task<ServiceResult<ProductDto>> GetByIdAsync(int id);
    Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<ServiceResult<ProductDto>> UpdateAsync(UpdateProductDto dto);
}
