# Final AI Usage Summary

## Tool used

**Cursor** — AI-assisted IDE with inline code generation, chat-based prompting, and codebase context awareness.

## How AI was used across phases

| Phase | AI role | Human role |
|-------|---------|------------|
| Planning | Drafted requirement breakdown and scope table | Selected trimmed core vs stretch; validated against spec |
| Design | Suggested Clean Architecture structure and entity list | Approved layering; defined calculation rules |
| Implementation | Generated project scaffold, entities, services, views, seed data | Reviewed logic; fixed CompanyId claim bug; corrected test math |
| Testing | Generated initial xUnit test templates | Fixed failing test expectations; ran `dotnet test` |
| Debugging | Suggested claim refresh pattern for Identity | Applied and verified fix in `AccountController` |
| Review | Identified layer violations and missing validations | Prioritized fixes; documented in `review-fixes.md` |
| Documentation | Drafted lifecycle markdown files | Added candidate info; verified accuracy against code |

## Prompts by phase

Detailed prompts are in `ai-prompts/`:

- `planning.md` — Scope and requirement prompts
- `design.md` — Architecture and data model prompts
- `implementation.md` — Code generation prompts
- `testing.md` — Test creation prompts
- `debugging.md` — Troubleshooting prompts
- `review.md` — Code review and documentation prompts

## Code ownership statement

All code was reviewed before commit. AI-generated suggestions were accepted, modified, or rejected based on:
- Correctness (e.g. quotation calculation order)
- Project conventions (ServiceResult pattern, CompanyId filtering)
- Security (no hardcoded secrets, authorize attributes)

Known manual fixes not generated correctly by AI:
1. `RefreshClaimsAsync` for CompanyId claim
2. Quotation calculator test expected values
3. `ThemeColorsViewComponent` for settings-driven UI

## Effectiveness assessment

**High value:** Solution scaffolding, boilerplate CRUD, Razor view structure, seed data, documentation drafts.

**Medium value:** Service implementations (needed review for tenant filtering), PDF layout.

**Low value / required human fix:** Identity claims, precise financial test assertions, assessment folder structure compliance.

## Lessons for future AI-assisted development

1. Scaffold required documentation artifacts before coding.
2. Use AI for repetitive structure; verify business logic manually.
3. Split prompt history by SDLC phase for traceability.
4. Run tests immediately after AI-generated calculation logic.

## Aggregate prompt count

Approximately 8–12 primary prompts across the build session, with follow-up refinements for bug fixes and documentation. See `prompt-history.md` for consolidated log and `ai-prompts/` for phase breakdown.
