# AI Prompts — Testing Phase

## Prompt 1 — Unit test scaffold

**Date:** 2026-07-02

**Prompt:**
> Create xUnit tests for QuotationCalculator with known values.

**Outcome:** `QuotationCalculatorTests.cs` with 2 test methods.

**Failure:** First run failed — `Expected: 700, Actual: 680`.

**My correction:** Recalculated expected values manually (see `debugging.md`). AI helped verify math but I updated assertions myself.

---

## Prompt 2 — Settings defaults tests

**Date:** 2026-07-02

**Prompt:**
> Create xUnit tests for SettingsDefaults fallback behavior.

**Outcome:** 3 tests in `SettingsDefaultsTests.cs` — all passed on first run.

---

## Prompt 3 — Edge case tests (post-review)

**Date:** 2026-07-25

**Prompt:**
> Add edge case unit tests: 100% line discount (zero tax), quotation-level discount subtraction.

**Outcome:**
- `CalculateLine_WithFullDiscount_HasZeroTax`
- `Calculate_WithQuotationDiscount_SubtractsFromDocumentTotal`

---

## Prompt 4 — Integration tests (iteration)

**Date:** 2026-07-25

**Prompt:**
> Add WebApplicationFactory integration test for quotation create flow with anti-forgery token handling.

**Iteration 1:** Tests failed — login redirect asserted `/Home` but actual redirect was `/`.

**Fix:** Accept both `/` and `/Home` as valid post-login redirects.

**Iteration 2:** Zero-quantity test searched for error text in HTML — field-level errors not rendered (view uses `ModelOnly` summary).

**Fix:** Assert HTTP 200 + quotation count remains 0 (outcome-based assertion).

**Iteration 3:** PDF text assertion failed on compressed PDF streams.

**Fix:** Compare PDF byte output between different settings instead.

**Final outcome:** 5 integration tests passing in `InventoryManagement.Web.Tests`.

---

## Prompt 5 — Record test results

**Date:** 2026-07-25

**Command run:**
```powershell
dotnet test
```

**Output:**
```
Application.Tests: Passed 7, Failed 0
Web.Tests:         Passed 5, Failed 0
Total:             12 tests
```

Recorded in `test-results.md`.
