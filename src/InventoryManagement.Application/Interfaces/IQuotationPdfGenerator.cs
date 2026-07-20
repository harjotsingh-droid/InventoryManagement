using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface IQuotationPdfGenerator
{
    byte[] Generate(QuotationPdfContextDto context);
}
