# AI Prompts — Documentation Phase

## Prompt 1 — README and setup docs

**Date:** 2026-07-02

**Prompt:**
> Write README.md for SME ERP Inventory Management with prerequisites, quick start, connection string setup, migration commands, seeded login credentials, test commands, and architecture notes.

**Outcome:**
- README with full setup instructions
- References lifecycle artifacts and solution structure

## Prompt 2 — Database setup notes

**Date:** 2026-07-02

**Prompt:**
> Create database/setup-notes.md documenting SQL Server LocalDB connection, migration commands, seed data summary, persistence verification steps, and troubleshooting.

**Outcome:**
- `database/setup-notes.md` with connection examples and seed table

## Prompt 3 — Lifecycle artifact scaffold

**Date:** 2026-07-25

**Prompt:**
> Create all required lifecycle markdown files: acceptance-criteria, implementation-plan, api-contract, data-model, ui-flow, design-notes, test-strategy, code-review-notes, review-fixes, pr-description, reflection, final-ai-usage-summary, tool-workflow, candidate-info.

**Outcome:**
- Full lifecycle documentation at repository root
- Content derived from actual implementation

## Prompt 4 — PR description

**Date:** 2026-07-25

**Prompt:**
> Write a pull request description for the SME ERP Inventory Management submission. Include summary, what changed, how to test, checklist, and related artifacts.

**Outcome:**
- Full PR description in `pr-description.md`

## Prompt 5 — Reflection and AI usage summary

**Date:** 2026-07-25

**Prompt:**
> Write reflection.md (what went well, challenges, lessons) and final-ai-usage-summary.md (how Cursor was used per phase, code ownership, effectiveness). Be honest about manual fixes.

**Outcome:**
- `reflection.md` and `final-ai-usage-summary.md` created
- Documents human fixes for CompanyId claim and test math

## Prompt 6 — Tool-specific Cursor workflow

**Date:** 2026-07-25

**Prompt:**
> Create tool-specific/cursor-workflow/ folder with Cursor IDE workflow artifacts: daily workflow, prompt conventions, and usage summary for assessment submission.

**Outcome:**
- `tool-specific/cursor-workflow/` with workflow and usage docs
