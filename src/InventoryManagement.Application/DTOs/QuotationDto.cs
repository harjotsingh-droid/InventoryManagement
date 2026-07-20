namespace InventoryManagement.Application.DTOs;

public class QuotationLineDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal GstPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class QuotationLineInputDto
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal GstPercent { get; set; }
}

public class QuotationDto
{
    public int Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public DateTime ValidUntil { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public IList<QuotationLineDto> Lines { get; set; } = new List<QuotationLineDto>();
}

public class CreateQuotationDto
{
    public int CustomerId { get; set; }
    public DateTime QuotationDate { get; set; } = DateTime.Today;
    public DateTime ValidUntil { get; set; } = DateTime.Today.AddDays(30);
    public decimal DiscountAmount { get; set; }
    public string? Notes { get; set; }
    public IList<QuotationLineInputDto> Lines { get; set; } = new List<QuotationLineInputDto>();
}

public class QuotationListItemDto
{
    public int Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public decimal TotalAmount { get; set; }
}

public class QuotationPdfContextDto
{
    public CompanySettingsDto Settings { get; set; } = new();
    public QuotationDto Quotation { get; set; } = new();
}
