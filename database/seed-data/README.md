# Seed Data

## Seeder implementation

Seed logic lives in:

```
src/InventoryManagement.Infrastructure/Seeding/DbSeeder.cs
```

Invoked on application startup. Applies migrations first, then seeds if no company exists.

## Seeded entities

### Company (1)

| Field | Value |
|-------|-------|
| Name | Demo Trading Co. |
| City | Ludhiana, Punjab, India |
| GSTIN | 03AABCD1234E1Z5 |
| PAN | AABCD1234E |

### Company settings (3)

| Key | Purpose |
|-----|---------|
| `PrimaryColor` | UI accent and PDF header color |
| `InvoiceTerms` | Terms text on quotation PDF |
| `InvoiceFooter` | Footer text on quotation PDF |

### Products (6)

| Name | SKU | Price (INR) | GST % | Stock |
|------|-----|-------------|-------|-------|
| Steel Bolt M8 | SB-M8-001 | 12.50 | 18 | 500 |
| Industrial Lubricant 1L | IL-1L-002 | 245.00 | 18 | 120 |
| Safety Gloves (Pair) | SG-PR-003 | 85.00 | 12 | 300 |
| PVC Pipe 2 inch | PVC-2IN-004 | 180.00 | 18 | 75 |
| Electric Motor 1HP | EM-1HP-005 | 8500.00 | 18 | 15 |
| Packaging Tape Roll | PT-RL-006 | 45.00 | 12 | 200 |

### Customers (4)

| Name | Code | City |
|------|------|------|
| Alpha Hardware Store | CUST-001 | Amritsar |
| Beta Engineering Works | CUST-002 | Jalandhar |
| Gamma Retail Mart | CUST-003 | Chandigarh |
| Delta Constructions | CUST-004 | Mohali |

### Users (2)

| Email | Password | Role |
|-------|----------|------|
| admin@demo.com | Admin@123 | Admin |
| user@demo.com | User@123 | User |

Both users belong to CompanyId = 1.

### Roles (2)

- Admin
- User

## Re-seed procedure

1. Delete database or drop all tables.
2. Restart application — `DbSeeder` runs `MigrateAsync()` then seeds.
3. Alternatively: `dotnet ef database drop` then `dotnet ef database update`.

## Verification

1. Login as `admin@demo.com`.
2. Products index shows 6 items.
3. Customers index shows 4 items.
4. Dashboard KPIs reflect seeded counts.

See `database/setup-notes.md` for persistence verification after restart.
