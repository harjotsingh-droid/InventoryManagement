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
├── InventoryManagement.Application.Tests/  # Unit tests (7)
└── InventoryManagement.Web.Tests/        # Integration tests (5)
database/
├── setup-notes.md
├── schema-or-migrations/
└── seed-data/
tool-specific/
└── cursor-workflow/
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

### Planning
| File | Purpose |
|------|---------|
| `candidate-info.md` | Candidate name, email, project metadata |
| `requirements-analysis.md` | Scope and business context |
| `acceptance-criteria.md` | Testable acceptance criteria (AC-01–AC-32) |
| `implementation-plan.md` | Phased build plan and definition of done |

### Design
| File | Purpose |
|------|---------|
| `api-contract.md` | MVC routes and service contracts |
| `data-model.md` | Entities, relationships, calculation rules |
| `ui-flow.md` | User journeys and navigation |
| `design-notes.md` | Architecture decisions and trade-offs |

### AI workflow
| File / Folder | Purpose |
|---------------|---------|
| `ai-prompts/planning.md` | Planning-phase prompts |
| `ai-prompts/design.md` | Design-phase prompts |
| `ai-prompts/implementation.md` | Implementation-phase prompts |
| `ai-prompts/testing.md` | Testing-phase prompts |
| `ai-prompts/debugging.md` | Debugging-phase prompts |
| `ai-prompts/code-review.md` | Code review-phase prompts |
| `ai-prompts/documentation.md` | Documentation-phase prompts |
| `prompt-history.md` | Consolidated prompt log |
| `final-ai-usage-summary.md` | AI tool usage summary |
| `tool-workflow.md` | Dev environment and commands |
| `tool-specific/cursor-workflow/` | Cursor IDE workflow artifacts |

### Testing and review
| File | Purpose |
|------|---------|
| `test-strategy.md` | Unit/manual test approach |
| `test-results.md` | `dotnet test` output |
| `testing-notes.md` | Manual test checklist |
| `debugging-notes.md` | Issues encountered and fixes |
| `code-review-notes.md` | Review findings |
| `review-fixes.md` | Bugs fixed after review |
| `code-review-reflection.md` | Earlier self-review notes |

### Documentation and ownership
| File | Purpose |
|------|---------|
| `pr-description.md` | Pull request summary |
| `reflection.md` | Project reflection |
| `demo-walkthrough.md` | Step-by-step demo with test evidence |
| `database/setup-notes.md` | Database setup and verification |
| `database/schema-or-migrations/` | Schema tables and migration commands |
| `database/seed-data/` | Demo seed data reference |

## Security

- Do not commit real connection strings with passwords
- Use user secrets or environment variables in local/dev
- Seeded passwords are for demo only — change in production

## Stretch (not implemented)

POS billing, purchases, payments, HR, Chart.js dashboard, user management UI, Docker/CI.
