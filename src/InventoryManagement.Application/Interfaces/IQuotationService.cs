using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface IQuotationService
{
    Task<ServiceResult<IReadOnlyList<QuotationListItemDto>>> GetAllAsync();
    Task<ServiceResult<QuotationDto>> GetByIdAsync(int id);
    Task<ServiceResult<QuotationDto>> CreateAsync(CreateQuotationDto dto);
    Task<ServiceResult<QuotationPdfContextDto>> GetPdfContextAsync(int id);
}
