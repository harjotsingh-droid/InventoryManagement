namespace InventoryManagement.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal GstPercent { get; set; }
    public int CurrentStock { get; set; }

    public Company Company { get; set; } = null!;
}
