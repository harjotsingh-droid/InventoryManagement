using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Identity;
using InventoryManagement.Infrastructure.Persistence;
using InventoryManagement.Shared.Constants;
using InventoryManagement.Shared.Theme;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Infrastructure.Seeding;

public static class DbSeeder
{
    public const string AdminEmail = "admin@demo.com";
    public const string AdminPassword = "Admin@123";
    public const string UserEmail = "user@demo.com";
    public const string UserPassword = "User@123";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        var context = provider.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }

        if (await context.Companies.AnyAsync())
        {
            logger.LogInformation("Database already seeded.");
            return;
        }

        var company = new Company
        {
            Name = "Demo Trading Co.",
            Tagline = "Quality products, trusted service",
            Address = "42 Industrial Estate, Phase 2",
            City = "Ludhiana",
            State = "Punjab",
            Country = "India",
            PinCode = "141003",
            GstNumber = "03AABCD1234E1Z5",
            PanNumber = "AABCD1234E",
            Mobile = "+91 98765 43210",
            Email = "sales@demotrading.com",
            Website = "https://demotrading.example"
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        context.CompanySettings.AddRange(
            new CompanySetting
            {
                CompanyId = company.Id,
                Key = SettingKeys.PrimaryColor,
                Value = ThemeHelper.DefaultPrimaryColor
            },
            new CompanySetting
            {
                CompanyId = company.Id,
                Key = SettingKeys.InvoiceTerms,
                Value = InventoryManagement.Application.Services.SettingsDefaults.DefaultInvoiceTerms
            },
            new CompanySetting
            {
                CompanyId = company.Id,
                Key = SettingKeys.InvoiceFooter,
                Value = InventoryManagement.Application.Services.SettingsDefaults.DefaultInvoiceFooter
            });

        context.Products.AddRange(
            new Product { CompanyId = company.Id, Name = "Steel Bolt M8", Sku = "SB-M8-001", Barcode = "8901001001001", SellingPrice = 12.50m, GstPercent = 18m, CurrentStock = 500 },
            new Product { CompanyId = company.Id, Name = "Industrial Lubricant 1L", Sku = "IL-1L-002", Barcode = "8901001001002", SellingPrice = 245.00m, GstPercent = 18m, CurrentStock = 120 },
            new Product { CompanyId = company.Id, Name = "Safety Gloves (Pair)", Sku = "SG-PR-003", Barcode = "8901001001003", SellingPrice = 85.00m, GstPercent = 12m, CurrentStock = 300 },
            new Product { CompanyId = company.Id, Name = "PVC Pipe 2 inch", Sku = "PVC-2IN-004", Barcode = "8901001001004", SellingPrice = 180.00m, GstPercent = 18m, CurrentStock = 75 },
            new Product { CompanyId = company.Id, Name = "Electric Motor 1HP", Sku = "EM-1HP-005", Barcode = "8901001001005", SellingPrice = 8500.00m, GstPercent = 18m, CurrentStock = 15 },
            new Product { CompanyId = company.Id, Name = "Packaging Tape Roll", Sku = "PT-RL-006", Barcode = "8901001001006", SellingPrice = 45.00m, GstPercent = 12m, CurrentStock = 200 });

        context.Customers.AddRange(
            new Customer { CompanyId = company.Id, Name = "Alpha Hardware Store", Code = "CUST-001", Mobile = "9876500001", City = "Amritsar", State = "Punjab", Address = "Main Bazaar Road" },
            new Customer { CompanyId = company.Id, Name = "Beta Engineering Works", Code = "CUST-002", Mobile = "9876500002", City = "Jalandhar", State = "Punjab", Address = "Focal Point" },
            new Customer { CompanyId = company.Id, Name = "Gamma Retail Mart", Code = "CUST-003", Mobile = "9876500003", City = "Chandigarh", State = "Chandigarh", Address = "Sector 34" },
            new Customer { CompanyId = company.Id, Name = "Delta Constructions", Code = "CUST-004", Mobile = "9876500004", City = "Mohali", State = "Punjab", Address = "Industrial Area" });

        await context.SaveChangesAsync();

        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        await EnsureRoleAsync(roleManager, "Admin");
        await EnsureRoleAsync(roleManager, "User");

        await EnsureUserAsync(userManager, AdminEmail, AdminPassword, "System Administrator", company.Id, "Admin");
        await EnsureUserAsync(userManager, UserEmail, UserPassword, "Sales User", company.Id, "User");

        logger.LogInformation("Database seeded successfully.");
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string fullName,
        int companyId,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return;
        }

        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = fullName,
            CompanyId = companyId
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
