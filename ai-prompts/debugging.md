# AI Prompts — Debugging Phase

## Prompt 1 — CompanyId claim issue

**Date:** 2026-07-02

**Prompt:**
> After login, product and customer lists are empty. Services filter by CompanyId but it seems to be 0. How do I add CompanyId claim from ApplicationUser in ASP.NET Identity?

**Outcome:**
- Identified missing claim on identity principal
- Suggested `RefreshClaimsAsync` pattern in AccountController
- Fix applied and verified — lists load after login

## Prompt 2 — Quotation test failure

**Date:** 2026-07-02

**Prompt:**
> QuotationCalculatorTests.Calculate_WithKnownLineItems_ProducesCorrectTotals is failing. The calculator applies line discount then GST. Help me recalculate expected values.

**Outcome:**
- Recalculated: lineAmount - discount = lineSubTotal; tax = lineSubTotal × gst%
- Updated test assertions; all tests pass

## Prompt 3 — Theme color not showing

**Date:** 2026-07-02

**Prompt:**
> Primary color saves in Settings but the navigation accent doesn't change. How can I inject the color into the layout from company settings?

**Outcome:**
- Created `ThemeColorsViewComponent`
- Invoked in `_Layout.cshtml` with CSS variable `--primary-color`

## Prompt 4 — PowerShell command chaining

**Date:** 2026-07-01

**Prompt:**
> dotnet commands with && fail in PowerShell. What's the alternative for chaining project scaffold commands?

**Outcome:**
- Use semicolon-separated commands
- Documented in `debugging-notes.md`

## Prompt 5 — EF Core decimal warnings

**Date:** 2026-07-02

**Prompt:**
> EF Core warns about decimal properties without precision. Configure HasPrecision for monetary columns in ApplicationDbContext.

**Outcome:**
- `HasPrecision(18, 2)` on price/total columns
- Build succeeds with 0 warnings
