# AI Prompts — Implementation Phase

## Prompt 1 — Solution bootstrap

**Date:** 2026-07-01

**Prompt:**
> Create full project based on Option 3 specification (.NET Full-Stack SME ERP — Inventory Management) including Clean Architecture, authentication, products, customers, quotations with PDF, settings-driven branding, global search, dashboard KPIs, EF Core migrations/seed, xUnit tests, and lifecycle documentation.

**Outcome:**
- Solution scaffolded: Domain, Application, Infrastructure, Shared, Web, Application.Tests
- Core entities, services, MVC UI, QuestPDF generator, Identity seeding
- Initial migration and DbSeeder

---

## Prompt 2 — Quotation calculator

**Date:** 2026-07-02

**Prompt:**
> Implement QuotationCalculator as a static class in Application layer. Calculate line totals: apply line discount first, then GST on discounted amount.

**Outcome:** `QuotationCalculator.cs` created.

**Rejected AI output:** First version applied GST on full line amount before discount. I rejected this after reading the spec's "discount-then-GST" rule and rewrote the calculation order manually.

---

## Prompt 3 — PDF generator (iteration 1 — rejected)

**Date:** 2026-07-02

**Prompt:**
> Implement QuotationPdfGenerator using QuestPDF with hardcoded company header "Demo Trading Co." and blue color.

**AI output:** Generated PDF with hardcoded branding strings in `QuotationPdfGenerator.cs`.

**Why rejected:** Violates acceptance criterion AC-22 (settings update must affect PDF). Hardcoded values would not reflect Settings changes.

**Follow-up prompt:**
> Re-implement QuotationPdfGenerator to accept QuotationPdfContextDto with settings from CompanySettingsService. No hardcoded company name or color.

**Final outcome:** `QuotationPdfGenerator.Generate(QuotationPdfContextDto context)` — settings-driven.

---

## Prompt 4 — Identity claims (iteration 1 — failed)

**Date:** 2026-07-02

**Prompt:**
> Users can log in but product list is empty. ApplicationUser has CompanyId property. Fix tenant filtering.

**AI suggestion (attempt 1):** Add `CompanyId` column to JWT token configuration.

**Why rejected:** Project uses cookie-based Identity, not JWT. Wrong authentication model.

**AI suggestion (attempt 2):** Filter products without CompanyId check temporarily.

**Why rejected:** Breaks multi-tenant isolation requirement.

**Manual fix I applied:** `RefreshClaimsAsync` in `AccountController` to add `CompanyId` claim after login. Verified products load.

---

## Prompt 5 — MVC controllers and views

**Date:** 2026-07-02

**Prompt:**
> Create MVC controllers for Products, Customers, Quotations (with dynamic line items), Settings, Search, and Home dashboard. Use ServiceResult pattern.

**Outcome:** All controllers with CRUD and search.

**Iteration:** Quotation Create view initially used server-side-only line items (no JS). Added client-side row cloning after manual testing showed poor UX for multi-line entry.

---

## Prompt 6 — Integration test project (post-review)

**Date:** 2026-07-25

**Prompt:**
> Add WebApplicationFactory integration tests for login and quotation create. Use InMemory database. Tests must pass with dotnet test.

**Iteration 1 failed:** `DbSeeder.MigrateAsync()` threw on InMemory provider.

**Fix I applied:**
- `Program.cs` skips seeder in `Testing` environment
- `DbSeeder` uses `EnsureCreatedAsync()` for non-relational databases
- `CustomWebApplicationFactory` swaps SQL Server for InMemory

**Outcome:** 5 integration tests passing.
