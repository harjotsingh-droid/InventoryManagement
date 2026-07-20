using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}

public class ProductFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Barcode { get; set; }

    [Range(0, double.MaxValue)]
    public decimal SellingPrice { get; set; }

    [Range(0, 100)]
    public decimal GstPercent { get; set; }

    [Range(0, int.MaxValue)]
    public int CurrentStock { get; set; }
}

public class CustomerFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Mobile { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }
}

public class QuotationLineFormViewModel
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal GstPercent { get; set; }
}

public class QuotationFormViewModel
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime QuotationDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ValidUntil { get; set; } = DateTime.Today.AddDays(30);

    [Range(0, double.MaxValue)]
    public decimal DiscountAmount { get; set; }

    public string? Notes { get; set; }

    public List<QuotationLineFormViewModel> Lines { get; set; } = new() { new QuotationLineFormViewModel() };
}

public class SettingsFormViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string? GstNumber { get; set; }
    public string? PanNumber { get; set; }
    public string? Mobile { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string PrimaryColor { get; set; } = "#2563eb";
    public string InvoiceTerms { get; set; } = string.Empty;
    public string InvoiceFooter { get; set; } = string.Empty;
}

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;
}
