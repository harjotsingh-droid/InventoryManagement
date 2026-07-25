# AI Prompts — Code Review Phase

## Prompt 1 — Architecture review

**Date:** 2026-07-02

**Prompt:**
> Review the Inventory Management solution for Clean Architecture violations. Check that controllers do not reference DbContext, Domain has no infrastructure dependencies, and all services filter by CompanyId.

**Outcome:**
- Layer separation confirmed good
- Findings documented in `code-review-notes.md`

## Prompt 2 — Security review

**Date:** 2026-07-02

**Prompt:**
> Review security: authentication on controllers, CSRF on POST actions, secrets handling, and demo password exposure. Produce findings table with severity.

**Outcome:**
- `[Authorize]` on business controllers verified
- No hardcoded production secrets
- Documented in `code-review-notes.md`

## Prompt 3 — Business logic review

**Date:** 2026-07-02

**Prompt:**
> Review quotation calculation logic, validation flow, SKU uniqueness checks, and PDF branding source. Are business rules centralized and tested?

**Outcome:**
- `QuotationCalculator` centralized and unit tested
- PDF reads live settings (not hardcoded)
- Validation via `ServiceResult<T>` pattern

## Prompt 4 — Review fixes documentation

**Date:** 2026-07-02

**Prompt:**
> Document all bugs found during review and their fixes: CompanyId claim, test expectations, theme color, PowerShell, decimal precision. Include files changed and verification steps.

**Outcome:**
- Five fixes documented with root cause and verification
- Saved to `review-fixes.md`

## Prompt 5 — Structure gate fix review

**Date:** 2026-07-25

**Prompt:**
> Assessment failed structure gate due to filename and folder mismatches. Verify all required scaffold files exist with exact names: requirements-analysis.md, ai-prompts/code-review.md, database subfolders, tool-specific/cursor-workflow.

**Outcome:**
- Renamed `requirement-analysis.md` → `requirements-analysis.md`
- Added missing prompt files and folder structure
