# Schema and Migrations

## Overview

| Item | Value |
|------|-------|
| Engine | Microsoft SQL Server |
| ORM | Entity Framework Core 8 |
| Database name | `InventoryManagementDb` |
| Migration | `InitialCreate` (2026-07-02) |

## Schema tables

### Business entities

| Table | Purpose |
|-------|---------|
| `Companies` | Company profile (name, address, GSTIN, PAN) |
| `CompanySettings` | Key-value settings (primary color, invoice terms/footer) |
| `Products` | Product catalog with SKU, price, GST, stock |
| `Customers` | Customer records with contact details |
| `Quotations` | Quotation header (number, dates, totals) |
| `QuotationLines` | Line items linked to products |

### Identity (ASP.NET Core)

| Table | Purpose |
|-------|---------|
| `AspNetUsers` | Users with `CompanyId` and `FullName` |
| `AspNetRoles` | Admin, User roles |
| `AspNetUserRoles` | User-role mapping |
| `AspNetUserClaims`, `AspNetRoleClaims` | Identity claims |
| `AspNetUserLogins`, `AspNetUserTokens` | External login support |

## Migration source location

EF Core migrations are maintained in the application project:

```
src/InventoryManagement.Infrastructure/Migrations/
├── 20260702052245_InitialCreate.cs
├── 20260702052245_InitialCreate.Designer.cs
└── ApplicationDbContextModelSnapshot.cs
```

DbContext: `src/InventoryManagement.Infrastructure/Persistence/ApplicationDbContext.cs`

## Commands

```powershell
# Apply migrations
dotnet ef database update --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web

# Add new migration (if schema changes)
dotnet ef migrations add <MigrationName> --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web --output-dir Migrations
```

## Decimal precision

Monetary columns configured with `HasPrecision(18, 2)` in `ApplicationDbContext`.

## Related docs

- `data-model.md` — entity relationships and calculation rules
- `database/setup-notes.md` — connection string and troubleshooting
- `database/seed-data/README.md` — demo seed data
