# AI Prompts — Implementation Phase

## Prompt 1 — Solution bootstrap

**Date:** 2026-07-01

**Prompt:**
> Create full project based on Option 3 specification (.NET Full-Stack SME ERP — Inventory Management) including Clean Architecture, authentication, products, customers, quotations with PDF, settings-driven branding, global search, dashboard KPIs, EF Core migrations/seed, xUnit tests, and lifecycle documentation.

**Outcome:**
- Solution scaffolded: Domain, Application, Infrastructure, Shared, Web, Application.Tests
- Core entities, services, MVC UI, QuestPDF generator, Identity seeding
- Initial migration and DbSeeder

## Prompt 2 — Quotation calculator

**Date:** 2026-07-02

**Prompt:**
> Implement QuotationCalculator as a static class in Application layer. Calculate line totals: apply line discount first, then GST on discounted amount. Document total = sum of lines minus quotation-level discount.

**Outcome:**
- `QuotationCalculator.cs` with `CalculateLine` and `Calculate` methods
- Pure logic, no database dependency

## Prompt 3 — PDF generator

**Date:** 2026-07-02

**Prompt:**
> Implement QuotationPdfGenerator using QuestPDF. Read company name, address, primary color, invoice terms, and footer from CompanySettingsService. Generate PDF with quotation lines and totals.

**Outcome:**
- `QuotationPdfGenerator.cs` in Infrastructure/Pdf
- Settings-driven branding (not hardcoded)

## Prompt 4 — Identity and seeding

**Date:** 2026-07-02

**Prompt:**
> Create DbSeeder that applies migrations and seeds: 1 company, 3 settings, 6 products, 4 customers, 2 users (admin/user) with roles. Skip seed if company exists.

**Outcome:**
- `DbSeeder.cs` with demo data
- Seeded credentials documented in README

## Prompt 5 — MVC controllers and views

**Date:** 2026-07-02

**Prompt:**
> Create MVC controllers for Products, Customers, Quotations (with dynamic line items), Settings, Search, and Home dashboard. Use ServiceResult pattern. Add ThemeColorsViewComponent for settings-driven accent color.

**Outcome:**
- All controllers with CRUD and search
- Razor views with validation
- Theme ViewComponent in layout
