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
