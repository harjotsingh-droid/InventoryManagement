namespace InventoryManagement.Application.DTOs;

public class CompanyProfileDto
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
}

public class CompanySettingsDto
{
    public CompanyProfileDto Profile { get; set; } = new();
    public string PrimaryColor { get; set; } = string.Empty;
    public string InvoiceTerms { get; set; } = string.Empty;
    public string InvoiceFooter { get; set; } = string.Empty;
}

public class UpdateCompanySettingsDto
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string? GstNumber { get; set; }
    public string? PanNumber { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string PrimaryColor { get; set; } = string.Empty;
    public string InvoiceTerms { get; set; } = string.Empty;
    public string InvoiceFooter { get; set; } = string.Empty;
}
