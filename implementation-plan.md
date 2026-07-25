# Implementation Plan

## Phase 1 — Planning and scaffolding (Day 1)

1. Read Option 3 specification and define trimmed core scope.
2. Document requirements in `requirement-analysis.md` and acceptance criteria.
3. Choose Clean Architecture layering: Domain → Application → Infrastructure → Web.
4. Create solution and project structure under `src/` and `tests/`.

**Deliverables:** Solution scaffold, requirement docs, design artifacts.

## Phase 2 — Domain and persistence (Day 1–2)

1. Define entities: `Company`, `CompanySetting`, `Product`, `Customer`, `Quotation`, `QuotationLine`.
2. Configure `ApplicationDbContext` with relationships and `CompanyId` filters.
3. Add EF Core migration (`InitialCreate`) and `DbSeeder` with demo data.
4. Integrate ASP.NET Identity with `ApplicationUser` and role seeding.

**Deliverables:** Migrations, seed data, database setup notes.

## Phase 3 — Application layer (Day 2)

1. Create DTOs and service interfaces (`IProductService`, `ICustomerService`, etc.).
2. Implement `ServiceResult<T>` for consistent validation errors.
3. Build `QuotationCalculator` for line and document totals.
4. Implement `SettingsDefaults` for fallback company settings.

**Deliverables:** Application services, DTOs, calculator logic.

## Phase 4 — Infrastructure services (Day 2–3)

1. Implement EF-backed services with `CompanyId` scoping via `ICurrentUserService`.
2. Build `QuotationPdfGenerator` using QuestPDF, reading settings from `CompanySettingsService`.
3. Register dependencies in `DependencyInjection.cs`.
4. Add `ThemeColorsViewComponent` for settings-driven UI accent.

**Deliverables:** Service implementations, PDF generator, DI wiring.

## Phase 5 — Web UI (Day 3)

1. `AccountController` — login/logout with `CompanyId` claim refresh.
2. `ProductsController`, `CustomersController` — CRUD with search.
3. `QuotationsController` — create with dynamic line items, detail, PDF download.
4. `SettingsController` — company profile and invoice settings.
5. `SearchController` — global search (min 2 chars).
6. `HomeController` — dashboard KPIs.
7. Shared layout with navigation and theme injection.

**Deliverables:** MVC controllers, Razor views, client-side line-item JS.

## Phase 6 — Testing and documentation (Day 3–4)

1. Write `QuotationCalculatorTests` and `SettingsDefaultsTests`.
2. Run `dotnet test` and record output in `test-results.md`.
3. Manual test checklist in `testing-notes.md`.
4. Document debugging issues in `debugging-notes.md`.
5. Self-review and lifecycle artifact completion.

**Deliverables:** Passing tests, lifecycle markdown files, README.

## Risk mitigations

| Risk | Mitigation |
|------|------------|
| LocalDB not available | Document SQL Express alternative in `database/setup-notes.md` |
| CompanyId claim missing | `RefreshClaimsAsync` on login |
| PDF branding hardcoded | Read from `CompanySettingsService` at generation time |
| Scope creep | Defer stretch features explicitly in docs |

## Definition of done

- All acceptance criteria marked ✅
- `dotnet build` and `dotnet test` succeed
- App runs locally with seeded login
- All required lifecycle artifacts present at repository root
