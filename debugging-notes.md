# Debugging Notes

## Issues and resolutions

### 1. PowerShell command chaining

**Symptom:**
```
The token '&&' is not a valid statement separator in this version.
```

**When:** Scaffolding solution with `dotnet new` chained commands in PowerShell 5.x.

**Before (failed):**
```powershell
dotnet new sln -n InventoryManagement && dotnet new classlib -n InventoryManagement.Domain
```

**After (worked):**
```powershell
dotnet new sln -n InventoryManagement; dotnet new classlib -n InventoryManagement.Domain
```

---

### 2. CompanyId claim on login — empty lists

**Symptom:** After successful login, Products and Customers pages showed zero rows. No exception thrown.

**Investigation trace:**
1. Set breakpoint in `ProductService.GetAllAsync()` — `_currentUser.CompanyId` returned `0`.
2. Inspected `HttpContext.User.Claims` — no `CompanyId` claim present.
3. Confirmed `ApplicationUser.CompanyId` was set to `1` in database.
4. ASP.NET Identity does not map custom user properties to claims automatically.

**Before (broken flow):**
```
Login → PasswordSignInAsync → Redirect → ProductService filters WHERE CompanyId = 0 → empty list
```

**Fix applied:** `AccountController.RefreshClaimsAsync` removes old claim, adds `CompanyId` from user record, re-signs in.

```csharp
await _userManager.AddClaimAsync(user, new Claim("CompanyId", user.CompanyId.ToString()));
await _signInManager.SignInAsync(user, isPersistent: false);
```

**After (working flow):**
```
Login → PasswordSignInAsync → RefreshClaimsAsync → CompanyId claim = 1 → products load
```

**Files:** `src/InventoryManagement.Web/Controllers/AccountController.cs`

---

### 3. Quotation calculator unit test failure

**Symptom:**
```
Assert.Equal() Failure: Expected: 700, Actual: 680
Test: QuotationCalculatorTests.Calculate_WithKnownLineItems_ProducesCorrectTotals
```

**Root cause:** Test author applied GST before line discount. Business rule is discount-then-GST.

**Manual recalculation:**
```
Line 1: (2 × 100) = 200; 10% discount = 20; subtotal = 180; 18% GST = 32.40; total = 212.40
Line 2: (1 × 500) = 500; 0% discount; subtotal = 500; 12% GST = 60; total = 560
Document: subtotal = 680; tax = 92.40; minus quote discount 50 = total 722.40
```

**Before (wrong expected values):** `SubTotal = 700`, `TotalAmount = 742.40`

**After (corrected):** `SubTotal = 680`, `TotalAmount = 722.40`

**Files:** `tests/InventoryManagement.Application.Tests/QuotationCalculatorTests.cs`

---

### 4. Primary color not applied in layout

**Symptom:** Settings page saved `#dc2626` but navigation bar stayed default blue after save.

**Investigation:**
1. Confirmed `CompanySettings` row updated in database.
2. `_Layout.cshtml` had hardcoded Bootstrap primary class, no dynamic CSS variable.
3. `ThemeColorsViewComponent` did not exist yet.

**Fix:** Created `ThemeColorsViewComponent` returning normalized color; layout injects:
```html
<style>:root { --primary-color: @Model.PrimaryColor; }</style>
```

**Files:** `ThemeColorsViewComponent.cs`, `Views/Shared/_Layout.cshtml`

---

### 5. EF Core decimal precision warnings

**Symptom:**
```
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No store type was specified for the decimal property 'SellingPrice' on entity type 'Product'.
```

**Fix:** Added `HasPrecision(18, 2)` on all monetary properties in `ApplicationDbContext.OnModelCreating`.

**Verification:** `dotnet build` — 0 warnings.

---

### 6. Integration tests — InMemory database seeding

**Symptom:** `WebApplicationFactory` tests failed — `MigrateAsync()` not supported on InMemory provider.

**Fix:**
1. Skip `DbSeeder` in `Testing` environment (`Program.cs`).
2. `DbSeeder` uses `EnsureCreatedAsync()` when database is not relational.
3. `CustomWebApplicationFactory` replaces SQL Server with InMemory and calls seeder.

**Verification:** `dotnet test` — 12 tests passed (7 unit + 5 integration).

---

## Useful commands

```powershell
dotnet build
dotnet test
dotnet ef database update --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web
dotnet run --project src/InventoryManagement.Web
```

## EF Core logging

Enable in `appsettings.Development.json`:

```json
"Logging": {
  "LogLevel": {
    "Microsoft.EntityFrameworkCore.Database.Command": "Information"
  }
}
```

Sample trace when loading products:
```
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[@__CompanyId_0='1'], CommandType='Text']
      SELECT [p].[Id], [p].[Name], ... FROM [Products] AS [p] WHERE [p].[CompanyId] = @__CompanyId_0
```
