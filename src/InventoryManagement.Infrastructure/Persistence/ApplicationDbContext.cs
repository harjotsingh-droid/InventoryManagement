using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanySetting> CompanySettings => Set<CompanySetting>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<QuotationLine> QuotationLines => Set<QuotationLine>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CompanySetting>()
            .HasIndex(s => new { s.CompanyId, s.Key })
            .IsUnique();

        builder.Entity<Product>()
            .HasIndex(p => new { p.CompanyId, p.Sku })
            .IsUnique();

        builder.Entity<Customer>()
            .HasIndex(c => new { c.CompanyId, c.Code })
            .IsUnique();

        builder.Entity<Quotation>()
            .HasIndex(q => new { q.CompanyId, q.QuotationNumber })
            .IsUnique();

        builder.Entity<Quotation>()
            .HasOne(q => q.Company)
            .WithMany(c => c.Quotations)
            .HasForeignKey(q => q.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Quotation>()
            .HasOne(q => q.Customer)
            .WithMany(c => c.Quotations)
            .HasForeignKey(q => q.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<QuotationLine>()
            .HasOne(l => l.Quotation)
            .WithMany(q => q.Lines)
            .HasForeignKey(l => l.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<QuotationLine>()
            .HasOne(l => l.Product)
            .WithMany()
            .HasForeignKey(l => l.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
