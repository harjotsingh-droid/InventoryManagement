using InventoryManagement.Application.Interfaces;
using InventoryManagement.Infrastructure.Identity;
using InventoryManagement.Infrastructure.Pdf;
using InventoryManagement.Infrastructure.Persistence;
using InventoryManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICompanySettingsService, CompanySettingsService>();
        services.AddScoped<IQuotationService, QuotationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IQuotationPdfGenerator, QuotationPdfGenerator>();

        return services;
    }
}
