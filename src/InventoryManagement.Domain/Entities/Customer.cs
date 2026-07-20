namespace InventoryManagement.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Mobile { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Address { get; set; }

    public Company Company { get; set; } = null!;
    public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
