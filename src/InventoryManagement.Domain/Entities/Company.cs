namespace InventoryManagement.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Tagline { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PinCode { get; set; }
    public string? GstNumber { get; set; }
    public string? PanNumber { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? LogoPath { get; set; }

    public ICollection<CompanySetting> Settings { get; set; } = new List<CompanySetting>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
