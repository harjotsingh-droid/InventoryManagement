# Cursor Workflow

## Tool

**Cursor** — AI-assisted IDE used for this SME ERP Inventory Management project.

## Session workflow

```
1. Open relevant source files for context
2. Prompt Cursor with specific constraints (Clean Architecture, ServiceResult pattern)
3. Review generated code — do not accept blindly
4. dotnet build → fix compile errors
5. dotnet test → fix test failures
6. Manual browser smoke test
7. Update lifecycle docs (debugging-notes, test-results)
8. Commit with descriptive message
```

## Effective prompting patterns

| Pattern | Example |
|---------|---------|
| Reference existing code | "Follow the same ServiceResult pattern as ProductService" |
| Constrain architecture | "No DbContext in Web layer" |
| Specify file targets | "Add method to IQuotationService and QuotationService" |
| Include error output | Paste build/test failure for targeted fix |
| Phase-specific docs | Save prompts to `ai-prompts/<phase>.md` |

## Context loading tips

- Open controller + service + entity before asking for CRUD changes
- Open `QuotationCalculator.cs` before asking for tax logic changes
- Open `DbSeeder.cs` before asking for seed data changes

## What Cursor handled well

- Solution and project scaffolding
- Boilerplate CRUD (controllers, views, DTOs)
- Razor view structure
- Documentation drafts
- Initial xUnit test templates

## What required manual intervention

- `CompanyId` claim refresh on login (`AccountController`)
- Quotation calculator test expected values
- `ThemeColorsViewComponent` for settings-driven UI
- Assessment folder/filename compliance

## Related artifacts

| File | Purpose |
|------|---------|
| `tool-specific/cursor-workflow/usage-summary.md` | AI usage by phase |
| `tool-specific/cursor-workflow/prompt-conventions.md` | Prompt templates |
| `ai-prompts/` | Phase-specific prompt history |
| `final-ai-usage-summary.md` | Root-level AI usage summary |
| `tool-workflow.md` | Dev environment and commands |
