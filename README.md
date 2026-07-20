# SME ERP — Inventory Management

A trimmed-core .NET full-stack ERP for trading and distribution companies. Built with Clean Architecture, ASP.NET Core MVC (Razor), EF Core, SQL Server, and ASP.NET Identity.

## Features (Core)

- **Authentication** — Login/logout with seeded users
- **Products** — List, search, create/edit
- **Customers** — List, create/edit
- **Quotations** — Create with line items, list, detail, PDF download
- **Settings** — Company profile, invoice terms/footer, primary color (drives PDF + UI theme)
- **Global search** — Products, customers, quotations (min 2 characters)
- **Dashboard** — KPI cards from live data

## Solution structure

```
src/
├── InventoryManagement.Domain/       # Entities
├── InventoryManagement.Application/  # DTOs, services, interfaces
├── InventoryManagement.Infrastructure/ # EF Core, Identity, PDF, seeding
├── InventoryManagement.Shared/       # Setting keys, theme helpers
└── InventoryManagement.Web/          # MVC controllers & Razor views
tests/
└── InventoryManagement.Application.Tests/
database/
└── setup-notes.md
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server LocalDB (included with Visual Studio) **or** SQL Server Express / full instance
- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

## Quick start

1. **Clone and restore**

   ```powershell
   cd "d:\Source Code\Inventory Management"
   dotnet restore
   ```

2. **Configure connection string**

   Edit `src/InventoryManagement.Web/appsettings.json` or use user secrets:

   ```powershell
   cd src/InventoryManagement.Web
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   ```

3. **Apply migrations** (also runs automatically on first startup via seeder)

   ```powershell
   dotnet ef database update --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web
   ```

4. **Run the app**

   ```powershell
   dotnet run --project src/InventoryManagement.Web
   ```

   Open `https://localhost:7xxx` (port shown in console).

5. **Log in**

   | Email | Password |
   |-------|----------|
   | admin@demo.com | Admin@123 |
   | user@demo.com | User@123 |

## Tests

```powershell
dotnet test
```

See `test-results.md` for recorded output.

## Architecture notes

- Controllers call application services only — no `DbContext` in MVC layer
- Services return `ServiceResult<T>` for validation and business failures
- Multi-tenant isolation via `CompanyId` on entities and user claims
- PDF branding reads from `Company` + `CompanySetting` (not hardcoded)
- QuestPDF (Community license) generates quotation PDFs

## Lifecycle artifacts

| File | Purpose |
|------|---------|
| `prompt-history.md` | AI prompt history for the project |
| `requirement-analysis.md` | Scope and acceptance mapping |
| `testing-notes.md` | Manual test checklist |
| `debugging-notes.md` | Issues encountered and fixes |
| `code-review-reflection.md` | Self-review and improvements |
| `test-results.md` | `dotnet test` output |
| `database/setup-notes.md` | Database setup and verification |

## Security

- Do not commit real connection strings with passwords
- Use user secrets or environment variables in local/dev
- Seeded passwords are for demo only — change in production

## Stretch (not implemented)

POS billing, purchases, payments, HR, Chart.js dashboard, user management UI, Docker/CI.
