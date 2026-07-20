using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
}
