# Demo Walkthrough

Step-by-step demonstration of the SME ERP Inventory Management application.

**Prerequisites:** .NET 8 SDK, SQL Server LocalDB, `dotnet restore` completed.

## 1. Start the application

```powershell
dotnet run --project src/InventoryManagement.Web
```

**Expected:** Console shows `Now listening on: https://localhost:7xxx`

**Evidence:** Application starts without build errors.

## 2. Login

1. Open browser to the HTTPS URL shown in console.
2. Redirected to `/Account/Login` (global authorize filter).
3. Enter credentials:

| Field | Value |
|-------|-------|
| Email | admin@demo.com |
| Password | Admin@123 |

**Expected:** Redirect to dashboard (`/Home/Index`) with KPI cards showing product, customer, and quotation counts.

**Automated evidence:** `Login_WithSeededCredentials_RedirectsToDashboard` integration test passes.

## 3. View products

1. Click **Products** in navigation.
2. Confirm 6 seeded products appear (Steel Bolt M8, Industrial Lubricant, etc.).
3. Search for `bolt` — only Steel Bolt M8 shown.

## 4. Create a quotation

1. Navigate to **Quotations → Create**.
2. Select customer: **Alpha Hardware Store**.
3. Add line item: **Steel Bolt M8**, Qty `10`, use default price/GST.
4. Click **Create Quotation**.

**Expected:** Redirect to quotation detail page with computed totals and quotation number (e.g. `QT-2026-0001`).

**Automated evidence:** `CreateQuotation_AfterLogin_PersistsAndRedirectsToDetails` integration test passes.

## 5. Download branded PDF

1. On quotation detail page, click **Download PDF**.
2. PDF opens with:
   - Company name: **Demo Trading Co.**
   - Company address from settings
   - Primary color header bar
   - Invoice terms and footer

## 6. Update settings and verify branding

1. Navigate to **Settings**.
2. Change **Primary Color** to `#dc2626` (red).
3. Update **Company Name** to `Demo Trading Co. (Updated)`.
4. Save settings.
5. Download the same quotation PDF again.

**Expected:** PDF header shows updated company name and red accent color.

**Automated evidence:** `Generate_WithDifferentCompanyNames_ProducesDifferentPdfOutput` test confirms settings change PDF output.

## 7. Global search

1. Enter `alpha` in the navigation search box.
2. Results show **Alpha Hardware Store** under Customers.

## 8. Run automated tests

```powershell
dotnet test
```

**Expected output (2026-07-25):**
```
Passed!  - Failed: 0, Passed: 7  (Application.Tests)
Passed!  - Failed: 0, Passed: 5  (Web.Tests)
Total: 12 tests
```

See `test-results.md` for full recorded output.

## 9. Verify persistence

1. Stop the app (`Ctrl+C`).
2. Run again: `dotnet run --project src/InventoryManagement.Web`
3. Login and confirm the quotation created in step 4 still exists.

## Validation edge case (automated)

`CreateQuotation_WithZeroQuantity_ReturnsValidationError` confirms zero-quantity submissions are rejected and not persisted.

## Screenshot placeholders

> Add screenshots here when demonstrating locally:
> - Dashboard KPIs after login
> - Quotation detail with line items
> - PDF with branded header
> - Settings page with color picker
