# AI Prompts — Planning Phase

## Prompt 1 — Project scope definition

**Date:** 2026-07-01

**Prompt:**
> I need to build Option 3: SME ERP — Inventory Management (.NET Full-Stack). Help me define the trimmed core scope vs stretch features. Include authentication, products, customers, quotations with PDF, settings, global search, dashboard KPIs, EF Core, SQL Server, and xUnit tests.

**Outcome:**
- Scoped trimmed core: login, products, customers, quotations, settings, search, dashboard
- Deferred stretch: POS, purchases, HR, Chart.js, Docker/CI
- Documented in `requirement-analysis.md`

## Prompt 2 — Acceptance criteria

**Date:** 2026-07-01

**Prompt:**
> Convert the trimmed core scope into testable acceptance criteria with checkboxes. Include technical criteria like no secrets in repo, README, and mandatory tests.

**Outcome:**
- Created acceptance criteria table with AC-01 through AC-32
- Saved to `acceptance-criteria.md`

## Prompt 3 — Implementation plan

**Date:** 2026-07-01

**Prompt:**
> Create a phased implementation plan for this ERP project: scaffolding, domain, application layer, infrastructure, web UI, testing. Include risks and definition of done.

**Outcome:**
- Six-phase plan with deliverables per phase
- Risk mitigations for LocalDB, CompanyId, PDF branding
- Saved to `implementation-plan.md`

## Prompt 4 — Candidate info template

**Date:** 2026-07-25

**Prompt:**
> Create candidate-info.md for assessment submission with name Harjot Singh, email harjot.singh@tothenew.com, project details, and repository layout showing all required lifecycle files.

**Outcome:**
- `candidate-info.md` with full metadata and folder structure
