# Pull Request Description

## Title

feat: SME ERP Inventory Management — trimmed core (Option 3)

## Summary

Implements a trimmed-core inventory and quotation management ERP for small trading companies using .NET 8 Clean Architecture. Includes authentication, product/customer CRUD, quotation workflow with branded PDF export, company settings, global search, dashboard KPIs, EF Core migrations with seed data, and xUnit tests.

## What changed

### Application code
- **Domain:** `Company`, `CompanySetting`, `Product`, `Customer`, `Quotation`, `QuotationLine` entities
- **Application:** DTOs, service interfaces, `QuotationCalculator`, `SettingsDefaults`, `ServiceResult<T>`
- **Infrastructure:** EF Core context, migrations, Identity, service implementations, QuestPDF generator, `DbSeeder`
- **Web:** MVC controllers and Razor views for all core features; `ThemeColorsViewComponent`

### Tests
- `QuotationCalculatorTests` — line and document total calculations
- `SettingsDefaultsTests` — default and override settings behavior
- All 5 tests passing

### Documentation
- Full lifecycle artifact scaffold (requirement analysis, design, testing, review, reflection)
- `ai-prompts/` folder with phase-specific prompt history
- README with setup instructions and seeded credentials

## Why

Assessment deliverable for Option 3 (SME ERP — Inventory Management). Focuses on demonstrable full-stack ERP fundamentals within trimmed core scope, deferring stretch features (POS, purchases, HR, CI/CD).

## How to test

1. `dotnet restore && dotnet build`
2. `dotnet test` — expect 5 passed
3. Configure SQL Server LocalDB connection string (see README)
4. `dotnet run --project src/InventoryManagement.Web`
5. Login: `admin@demo.com` / `Admin@123`
6. Create quotation → download PDF → update Settings → verify PDF branding changes

## Screenshots / evidence

- `test-results.md` — automated test output
- `testing-notes.md` — manual test checklist
- `database/setup-notes.md` — migration and persistence verification

## Checklist

- [x] Builds without errors
- [x] All unit tests pass
- [x] No secrets in repository
- [x] README documents setup
- [x] Lifecycle documentation complete
- [x] Acceptance criteria met (see `acceptance-criteria.md`)

## Related artifacts

| File | Purpose |
|------|---------|
| `requirements-analysis.md` | Scope mapping |
| `acceptance-criteria.md` | Testable criteria |
| `implementation-plan.md` | Build phases |
| `data-model.md` | Entity design |
| `api-contract.md` | MVC routes and services |
| `ui-flow.md` | User journeys |
| `design-notes.md` | Architecture decisions |
