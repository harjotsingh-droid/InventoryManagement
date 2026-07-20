namespace InventoryManagement.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal GstPercent { get; set; }
    public int CurrentStock { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal GstPercent { get; set; }
    public int CurrentStock { get; set; }
}

public class UpdateProductDto : CreateProductDto
{
    public int Id { get; set; }
}
