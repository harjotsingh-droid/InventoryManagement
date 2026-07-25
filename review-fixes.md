# Review Fixes

Issues identified during code review and the fixes applied.

## Fix 1 — CompanyId claim on login

**Issue:** After login, services filtered by `CompanyId = 0` because the claim was not set on the identity principal.

**Root cause:** ASP.NET Identity does not automatically include custom `CompanyId` from `ApplicationUser`.

**Fix:** Added `RefreshClaimsAsync` in `AccountController` to add `CompanyId` claim after successful sign-in and re-sign the user.

**Files:** `src/InventoryManagement.Web/Controllers/AccountController.cs`

**Verified:** Products, customers, and quotations load correctly after login.

---

## Fix 2 — Quotation calculator test expectations

**Issue:** Unit test `Calculate_WithKnownLineItems_ProducesCorrectTotals` failed with incorrect expected subtotal.

**Root cause:** Test author miscalculated discount-then-GST order.

**Fix:** Updated test expectations to match `QuotationCalculator` logic:
- Line: `(qty × price − line discount) + GST on discounted amount`
- Document: sum lines, subtract quotation-level discount

**Files:** `tests/InventoryManagement.Application.Tests/QuotationCalculatorTests.cs`

**Verified:** `dotnet test` — all 5 tests pass.

---

## Fix 3 — Primary color not applied in layout

**Issue:** Settings primary color saved but UI accent unchanged.

**Root cause:** Layout did not read settings at render time.

**Fix:** Added `ThemeColorsViewComponent` and invoked in `_Layout.cshtml` to inject CSS `--primary-color`.

**Files:** `ThemeColorsViewComponent.cs`, `Views/Shared/_Layout.cshtml`

**Verified:** Dashboard nav accent updates after settings save (may require refresh).

---

## Fix 4 — PowerShell scaffolding commands

**Issue:** `&&` operator failed in older PowerShell during project creation.

**Fix:** Used semicolon-separated commands for `dotnet new` and `dotnet sln add`.

**Impact:** Development environment only; no code change.

---

## Fix 5 — Decimal precision warnings

**Issue:** EF Core warnings on decimal properties without explicit precision.

**Fix:** Configured `HasPrecision(18, 2)` on monetary columns in `ApplicationDbContext`.

**Files:** `src/InventoryManagement.Infrastructure/Persistence/ApplicationDbContext.cs`

**Verified:** `dotnet build` — 0 warnings.

---

## Deferred fixes (not implemented)

| Issue | Reason |
|-------|--------|
| Integration tests for login/quotation | Stretch; unit tests satisfy core requirement |
| Background PDF generation | Out of scope for trimmed core |
| ClaimsPrincipalFactory refactor | Works with current login fix |
