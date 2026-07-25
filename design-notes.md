# Design Notes

## Architecture decision: Clean Architecture

**Decision:** Four-layer structure — Domain, Application, Infrastructure, Web.

**Rationale:** Keeps business rules testable without database or HTTP dependencies. Controllers stay thin; EF Core and QuestPDF are infrastructure concerns.

**Trade-off:** More projects and boilerplate than a single-layer MVC app, but aligns with assessment expectations and supports future API extraction.

## Architecture decision: ServiceResult pattern

**Decision:** Services return `ServiceResult<T>` instead of throwing exceptions for validation failures.

**Rationale:** Predictable error flow from services → controllers → ModelState/TempData without try/catch in every action.

## Architecture decision: MVC over Web API

**Decision:** Server-rendered Razor views instead of SPA + REST API.

**Rationale:** Faster delivery for trimmed core scope; assessment focuses on full-stack ERP features, not frontend framework choice.

## Multi-tenancy design

**Decision:** `CompanyId` column on all business entities; filtered in every service query via `ICurrentUserService`.

**Rationale:** Demonstrates tenant isolation pattern without full multi-tenant infrastructure (subdomains, tenant resolver middleware).

**Limitation:** Demo seeds single company (Id = 1); all users share same tenant.

## PDF generation design

**Decision:** QuestPDF Community license; synchronous generation in HTTP request.

**Rationale:** No external PDF service dependency; settings-driven branding proves cross-layer configuration.

**Data flow:**
```
Settings form → CompanySettingsService → DB
                                         ↓
Quotation PDF request → QuotationPdfGenerator → reads Company + CompanySetting → byte[]
```

## Quotation calculation design

**Decision:** Pure static `QuotationCalculator` in Application layer.

**Rationale:** Unit-testable without mocking database; same logic used by service on save and tests.

**Rule:** Line discount applied before GST (INR-style indirect tax calculation).

## Identity and claims

**Decision:** Manual `CompanyId` claim refresh in `AccountController` after login.

**Rationale:** Ensures services always receive correct tenant ID.

**Improvement (deferred):** `IClaimsPrincipalFactory` for automatic claim injection.

## Theme injection

**Decision:** `ThemeColorsViewComponent` invoked in `_Layout.cshtml` to set CSS `--primary-color` variable.

**Rationale:** Settings change affects both PDF and live UI without hardcoded colors.

## Deferred design items (stretch)

| Item | Reason deferred |
|------|-----------------|
| Background PDF queue | Not needed for demo volume |
| Integration tests | Core scope requires unit tests only |
| Stock decrement on sale | No sales invoice in trimmed core |
| Logo file upload | LogoPath column reserved, UI not built |
| CI/CD pipeline | Stretch requirement |

## Technology choices

| Component | Choice | Alternative considered |
|-----------|--------|------------------------|
| ORM | EF Core 8 | Dapper (more manual mapping) |
| PDF | QuestPDF | iText (licensing), Rotativa (wkhtmltopdf dependency) |
| Auth | ASP.NET Identity | Custom auth (reinventing wheel) |
| Tests | xUnit | NUnit (either acceptable) |
| DB | SQL Server LocalDB | SQLite (less realistic for ERP) |
