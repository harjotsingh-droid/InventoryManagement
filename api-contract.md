# API Contract

This project is an ASP.NET Core MVC application (server-rendered Razor views), not a REST API. The contract below describes MVC routes, HTTP methods, and service-layer operations.

## Authentication

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/Account/Login` | GET | Anonymous | Login form |
| `/Account/Login` | POST | Anonymous | Authenticate user; sets `CompanyId` claim |
| `/Account/Logout` | POST | Required | Sign out |

**Seeded credentials:** `admin@demo.com` / `Admin@123`, `user@demo.com` / `User@123`

## Dashboard

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/` or `/Home/Index` | GET | Required | KPI cards: product, customer, quotation counts |

## Products

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/Products` | GET | Required | List products; optional `?search=` filter |
| `/Products/Create` | GET | Required | Create form |
| `/Products/Create` | POST | Required | Create product; validates unique SKU |
| `/Products/Edit/{id}` | GET | Required | Edit form |
| `/Products/Edit/{id}` | POST | Required | Update product |

**Service:** `IProductService` — `GetAllAsync(search?)`, `GetByIdAsync(id)`, `CreateAsync(dto)`, `UpdateAsync(dto)`

## Customers

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/Customers` | GET | Required | List customers |
| `/Customers/Create` | GET/POST | Required | Create customer |
| `/Customers/Edit/{id}` | GET/POST | Required | Edit customer |

**Service:** `ICustomerService` — `GetAllAsync()`, `GetByIdAsync(id)`, `CreateAsync(dto)`, `UpdateAsync(dto)`

## Quotations

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/Quotations` | GET | Required | List quotations |
| `/Quotations/Details/{id}` | GET | Required | Quotation detail with lines |
| `/Quotations/Create` | GET/POST | Required | Create quotation with line items |
| `/Quotations/DownloadPdf/{id}` | GET | Required | Returns `application/pdf` |

**Service:** `IQuotationService` — `GetAllAsync()`, `GetByIdAsync(id)`, `CreateAsync(dto)`

**PDF:** `IQuotationPdfGenerator.GenerateAsync(quotationId)` — reads company settings for branding.

## Settings

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/Settings` | GET | Required | Company profile and invoice settings form |
| `/Settings` | POST | Required | Save settings; affects next PDF and UI theme |

**Service:** `ICompanySettingsService` — `GetAsync()`, `UpdateAsync(dto)`

**Setting keys:** `PrimaryColor`, `InvoiceTerms`, `InvoiceFooter`

## Global search

| Route | Method | Auth | Description |
|-------|--------|------|-------------|
| `/Search` | GET | Required | Search across products, customers, quotations; `?q=` (min 2 chars) |

**Service:** `ISearchService.SearchAsync(query)`

## Service result contract

All application services return `ServiceResult<T>`:

```csharp
public class ServiceResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public IDictionary<string, string[]>? ValidationErrors { get; init; }
}
```

Controllers map `ValidationErrors` to `ModelState` and `ErrorMessage` to `TempData["Error"]`.

## Multi-tenancy

All data queries filter by `CompanyId` from the authenticated user's claim. Services use `ICurrentUserService.GetCompanyId()`.
