# Requirement Analysis

## Business context

Small trading/distribution company needs internal ERP for catalog management, customer records, quotation workflow, and branded PDF output.

## Trimmed core scope (in scope)

| Requirement | Implementation |
|-------------|----------------|
| Login / logout | ASP.NET Identity, AccountController, seeded users |
| Products list + search | ProductsController, ProductService keyword filter |
| Products optional create/edit | Create/Edit views and forms |
| Customers list | CustomersController, CustomerService |
| Customers optional create/edit | Create/Edit views |
| Quotations CRUD workflow | Create with lines, list, detail, PDF download |
| Settings | Company profile + invoice terms/footer + primary color |
| Settings → PDF consistency | QuotationPdfGenerator reads CompanySettingsService |
| Global search | SearchController, min 2 chars, products/customers/quotations |
| Dashboard KPIs | Home/Index with product, customer, quotation counts |
| EF Core + SQL Server | ApplicationDbContext, migrations, LocalDB connection |
| Validation | ServiceResult + ModelState in UI |
| Multi-tenant CompanyId | On entities and user claim |
| 2 xUnit tests | QuotationCalculatorTests, SettingsDefaultsTests |

## Stretch (deferred)

POS, purchases, suppliers, payments, HR, user management UI, Chart.js, stock audit, Docker/CI.

## Acceptance criteria mapping

- [x] Seeded login
- [x] Product list/search
- [x] Customer list
- [x] Quotation create with multiple lines
- [x] Quotation list and detail
- [x] PDF download from settings
- [x] Settings update affects next PDF
- [x] Global search
- [x] Persistence via SQL Server
- [x] Backend quotation validation
- [x] UI error states
- [x] No secrets in repo (placeholder connection string)
- [x] README setup instructions
- [x] Mandatory tests

## Assumptions

- Single company per deployment for demo (CompanyId = 1 for all seeded data)
- INR-style tax calculation: line discount then GST on discounted amount
- QuestPDF Community license acceptable for academic/demo use

## Edge cases

| Scenario | Expected behavior | Implementation |
|----------|-------------------|----------------|
| Login with wrong password | Error message on login form | `AccountController` ModelState error |
| Login without CompanyId claim | Empty product/customer lists | `RefreshClaimsAsync` adds claim on login |
| Duplicate product SKU | Validation error, no save | `ProductService` uniqueness check |
| Quotation with zero quantity | Rejected server-side, not saved | `QuotationService.ValidateCreate` |
| Quotation with missing customer | Rejected with field error | `CustomerId` validation |
| Valid-until before quotation date | Rejected with field error | Date comparison in service |
| Line discount 100% | Line subtotal and tax = 0 | `QuotationCalculator` unit tested |
| Quotation-level discount | Subtracted from document total after line taxes | `QuotationCalculator.Calculate` |
| Global search with 1 character | Validation message, no results | `SearchController` min 2 chars |
| Settings color change | UI accent + next PDF reflect new color | `ThemeColorsViewComponent` + `QuotationPdfGenerator` |
| PDF for non-existent quotation | Redirect with error | `QuotationsController.DownloadPdf` |
| Restart app after creating data | Data persists in SQL Server | EF Core migrations + seed |

## Open questions (resolved for demo)

| Question | Decision |
|----------|----------|
| Multi-company per deployment? | Deferred — single CompanyId = 1 for demo |
| Stock decrement on quotation? | Out of scope — stock is informational only |
| Async PDF generation? | Deferred — synchronous for demo volume |
| GST inclusive vs exclusive pricing? | Exclusive — GST added on discounted line amount |
| Integration tests required? | Added — `WebApplicationFactory` for login and quotation create |
