# AI Prompts — Testing Phase

## Prompt 1 — Unit test scaffold

**Date:** 2026-07-02

**Prompt:**
> Create xUnit tests for QuotationCalculator: test line calculation with discount and GST, and document totals with multiple lines and quotation-level discount. Use known values for assertions.

**Outcome:**
- `QuotationCalculatorTests.cs` with 2 test methods
- Initial test failed due to incorrect expected values (fixed manually)

## Prompt 2 — Settings defaults tests

**Date:** 2026-07-02

**Prompt:**
> Create xUnit tests for SettingsDefaults: verify default values when settings keys are missing, verify configured values when present, and test GetSetting fallback.

**Outcome:**
- `SettingsDefaultsTests.cs` with 3 test methods
- All tests pass

## Prompt 3 — Test strategy document

**Date:** 2026-07-02

**Prompt:**
> Write test-strategy.md covering unit tests, manual checklist, deferred integration tests, pass/fail criteria, and known gaps.

**Outcome:**
- Test strategy with levels, coverage focus, and execution record reference

## Prompt 4 — Record test results

**Date:** 2026-07-02

**Prompt:**
> Run dotnet test and record output in test-results.md with test names and pass/fail table.

**Outcome:**
- 5 tests passed, 0 failed
- Output saved to `test-results.md`

## Prompt 5 — Manual test checklist

**Date:** 2026-07-02

**Prompt:**
> Create manual test checklist for authentication, products, customers, quotations, settings/PDF, global search, and persistence after restart.

**Outcome:**
- Checklist saved to `testing-notes.md`
