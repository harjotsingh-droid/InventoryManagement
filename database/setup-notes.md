# Database Setup Notes

## Database choice

| Item | Value |
|------|-------|
| Engine | Microsoft SQL Server |
| Local dev | SQL Server LocalDB `(localdb)\mssqllocaldb` |
| ORM | Entity Framework Core 8 |
| Database name | `InventoryManagementDb` |

## Connection string (example — no real passwords)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

For SQL Server Express:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=InventoryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

## Migration command

From the repository root:

```powershell
dotnet ef migrations add InitialCreate --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web --output-dir Migrations
dotnet ef database update --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web
```

Migrations are stored in `src/InventoryManagement.Infrastructure/Migrations/`.

## Seed data

On first startup, `DbSeeder` applies migrations and seeds:

| Entity | Count | Notes |
|--------|-------|-------|
| Company | 1 | Demo Trading Co. with address, GSTIN, PAN |
| CompanySetting | 3 | Primary color, invoice terms, invoice footer |
| Product | 6 | Mixed GST rates and stock levels |
| Customer | 4 | Punjab/Chandigarh region |
| Users | 2 | admin@demo.com (Admin), user@demo.com (User) |
| Roles | 2 | Admin, User |

Seeder skips if a company already exists.

## Verify persistence after restart

1. Run the app and log in as `admin@demo.com`.
2. Create a quotation or edit Settings (e.g. change primary color).
3. Stop the application (`Ctrl+C`).
4. Run again: `dotnet run --project src/InventoryManagement.Web`
5. Confirm data is still present (products, customers, quotations, settings).

## Troubleshooting

- **LocalDB not installed**: Install [SQL Server Express LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) or point the connection string to an existing SQL Server instance.
- **Migration pending**: Run `dotnet ef database update` manually.
- **Login works but empty data**: Check that seed ran — delete the database and restart to re-seed.
