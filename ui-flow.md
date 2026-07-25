# UI Flow

## Navigation structure

```
[Login]
   ↓ (success)
[Dashboard] ──→ Products ──→ Create / Edit
            ──→ Customers ──→ Create / Edit
            ──→ Quotations ──→ Create / Details / Download PDF
            ──→ Settings
            ──→ Search (global)
            ──→ Logout
```

All pages except Login require authentication. Layout includes top navigation, global search box, and theme accent from company settings.

## Flow 1 — Login

1. User opens `/Account/Login`.
2. Enters email and password.
3. On success: `CompanyId` claim added, redirect to Dashboard.
4. On failure: validation error on form.

## Flow 2 — Product management

1. User navigates to **Products**.
2. Optional: enter search keyword → filtered list.
3. **Create:** fill Name, SKU, price, GST, stock → POST → redirect to list.
4. **Edit:** modify fields → POST → redirect to list.
5. Duplicate SKU shows field-level error.

## Flow 3 — Customer management

1. User navigates to **Customers**.
2. **Create/Edit:** fill Name, Code, contact fields → save → list refresh.

## Flow 4 — Quotation creation (primary workflow)

1. User navigates to **Quotations → Create**.
2. Selects customer from dropdown.
3. Sets quotation date and valid-until date.
4. Adds line items (JavaScript clones rows):
   - Select product (auto-fills price and GST)
   - Enter quantity and optional line discount
5. Optional: quotation-level discount and notes.
6. Submit → server validates (customer required, qty > 0) → `QuotationCalculator` computes totals → save.
7. Redirect to **Details** page showing lines and totals.
8. User clicks **Download PDF** → branded PDF with company address, color, terms, footer.

## Flow 5 — Settings and PDF branding

1. User navigates to **Settings**.
2. Updates company address, primary color, invoice terms, footer.
3. Save → settings persisted to `CompanySetting` table.
4. UI accent updates via `ThemeColorsViewComponent`.
5. Next PDF download reflects new branding.

## Flow 6 — Global search

1. User enters query (min 2 characters) in nav search box.
2. Results page groups matches: Products, Customers, Quotations.
3. Single character shows validation message.

## Flow 7 — Dashboard

1. User lands on **Home/Index** after login.
2. Three KPI cards show live counts from database:
   - Total products
   - Total customers
   - Total quotations

## Error handling patterns

| Scenario | UI behavior |
|----------|-------------|
| Validation failure | Red field errors via `ModelState` |
| Business rule failure | `TempData["Error"]` banner on redirect |
| Not found | Redirect to list with error message |
| Unauthenticated | Redirect to Login |

## Key views

| View | Path |
|------|------|
| Login | `Views/Account/Login.cshtml` |
| Dashboard | `Views/Home/Index.cshtml` |
| Product list | `Views/Products/Index.cshtml` |
| Quotation create | `Views/Quotations/Create.cshtml` |
| Quotation detail | `Views/Quotations/Details.cshtml` |
| Settings | `Views/Settings/Index.cshtml` |
| Search results | `Views/Search/Index.cshtml` |
