# AI Prompts — Debugging Phase

## Prompt 1 — CompanyId claim issue (3 iterations)

**Date:** 2026-07-02

**Symptom I reported:**
> After login, product and customer lists are empty. No errors in console. Using ASP.NET Identity with ApplicationUser.CompanyId.

**AI attempt 1:**
> Check that DbSeeder set CompanyId on user records.

**My verification:** CompanyId = 1 in database. **Not the root cause.**

**AI attempt 2:**
> Add `.Where(p => true)` temporarily to ProductService to show all products.

**My decision:** **Rejected** — violates tenant isolation. Did not apply.

**AI attempt 3:**
> Add CompanyId as a claim in AccountController after successful login using UserManager.AddClaimAsync.

**My verification:** Applied `RefreshClaimsAsync`. Products load. **Accepted.**

**Trace evidence:** `_currentUser.CompanyId` was `0` before fix, `1` after.

---

## Prompt 2 — Quotation test failure

**Date:** 2026-07-02

**Symptom:**
```
Assert.Equal() Failure: Expected: 700, Actual: 680
```

**Prompt:**
> QuotationCalculatorTests is failing. Calculator applies line discount then GST. Help recalculate expected values for lines (2, 100, 10%, 18%) and (1, 500, 0%, 12%) with quote discount 50.

**AI output:** Provided step-by-step math confirming 680 subtotal.

**My action:** Updated test assertions manually. Did not change calculator code — calculator was correct, test expectations were wrong.

---

## Prompt 3 — Theme color not showing

**Date:** 2026-07-02

**Symptom:** Settings saved `#dc2626` but nav bar stayed blue.

**AI suggestion:** Inject CSS variable via ViewComponent reading from CompanySettingsService.

**Outcome:** Created `ThemeColorsViewComponent`. Required page refresh after save — documented as known limitation.

---

## Prompt 4 — Integration test InMemory failure

**Date:** 2026-07-25

**Symptom:**
```
System.InvalidOperationException: Relational-specific methods can only be used with relational databases.
```

**At:** `DbSeeder.SeedAsync` → `MigrateAsync()` on InMemory provider.

**AI suggestion:** Check `context.Database.IsRelational()` before calling Migrate.

**My fix:**
```csharp
if (context.Database.IsRelational())
    await context.Database.MigrateAsync();
else
    await context.Database.EnsureCreatedAsync();
```

Also added `Testing` environment guard in `Program.cs`.

**Verification:** `dotnet test` — 12/12 passed.

---

## Prompt 5 — PDF text assertion failure in tests

**Date:** 2026-07-25

**Symptom:** `Assert.Contains("Acme Custom Branding Ltd", pdfText)` failed — QuestPDF compresses text streams.

**AI suggestion:** Compare PDF byte output between two different company names instead of parsing text.

**Outcome:** `Generate_WithDifferentCompanyNames_ProducesDifferentPdfOutput` — asserts different settings produce different PDF bytes.
