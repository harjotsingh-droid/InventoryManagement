# Cursor Usage Summary

**Candidate:** Harjot Singh  
**Project:** SME ERP — Inventory Management (Option 3)  
**AI Tool:** Cursor  
**Date:** 2026-07-25

## Usage by SDLC phase

| Phase | Cursor contribution | Human contribution |
|-------|--------------------|--------------------|
| Planning | Scope tables, acceptance criteria drafts | Selected trimmed core vs stretch |
| Design | Architecture scaffold, entity suggestions | Approved calculation rules, layering |
| Implementation | Project scaffold, services, views, seed data | Fixed CompanyId claim, reviewed logic |
| Testing | xUnit test templates | Corrected test math, ran `dotnet test` |
| Debugging | Suggested claim refresh pattern | Applied and verified fixes |
| Code review | Findings tables, improvement list | Prioritized fixes in `review-fixes.md` |
| Documentation | Lifecycle markdown drafts | Verified accuracy, added candidate info |

## Prompt count

Approximately 8–12 primary prompts with follow-up refinements. Full history in `ai-prompts/` (7 phase files).

## Code ownership

All AI-generated code was reviewed before commit. Accepted when correct; modified or rejected when it violated project conventions or business rules.

## Manual fixes (not AI-generated correctly)

1. `RefreshClaimsAsync` for CompanyId claim on login
2. Quotation calculator test expected values
3. `ThemeColorsViewComponent` for settings-driven UI accent
4. Repository structure/filename compliance for assessment

## Effectiveness rating

| Area | Rating |
|------|--------|
| Scaffolding speed | High |
| CRUD boilerplate | High |
| Business logic accuracy | Medium (needs verification) |
| Identity/claims | Low (required manual fix) |
| Assessment structure compliance | Low (required manual scaffold) |

## Lessons learned

1. Create required folder/file scaffold before coding.
2. Match assessment filenames exactly.
3. Run tests immediately after AI-generated calculation logic.
4. Keep phase-specific prompt files for traceability.
