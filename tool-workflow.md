# Tool Workflow

## Development environment

| Tool | Version / details | Purpose |
|------|-------------------|---------|
| .NET SDK | 8.0 | Build, run, test |
| Visual Studio / VS Code | Latest | IDE |
| Cursor | AI-assisted IDE | Code generation, review, documentation |
| SQL Server LocalDB | `(localdb)\mssqllocaldb` | Local database |
| EF Core CLI | `dotnet-ef` global tool | Migrations |
| Git | Version control | Source management |
| PowerShell | Windows shell | Commands and scripts |

## Daily workflow

```
1. Plan feature / fix → update lifecycle docs if needed
2. Prompt AI in Cursor with context (open files, spec reference)
3. Review generated code → build locally
4. dotnet test → fix failures
5. Manual smoke test in browser
6. Update debugging-notes.md / test-results.md if issues found
7. Commit with descriptive message
```

## Key commands

```powershell
# Restore and build
dotnet restore
dotnet build

# Run tests
dotnet test

# Database migration
dotnet ef database update --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web

# Run application
dotnet run --project src/InventoryManagement.Web

# User secrets (connection string)
cd src/InventoryManagement.Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

## AI tool workflow (Cursor)

1. **Context loading:** Open relevant files (controller, service, entity) before prompting.
2. **Specific prompts:** Reference file names, patterns (`ServiceResult<T>`), and constraints (no DbContext in Web).
3. **Iterative refinement:** Generate → build → fix errors → re-prompt with error output.
4. **Documentation pass:** Final session to create missing lifecycle artifacts from existing code and notes.

## Project structure conventions

| Path | Convention |
|------|------------|
| `src/InventoryManagement.Domain/` | Entities only, no dependencies |
| `src/InventoryManagement.Application/` | Interfaces, DTOs, pure logic |
| `src/InventoryManagement.Infrastructure/` | EF Core, Identity, PDF, seeding |
| `src/InventoryManagement.Web/` | Controllers, views, ViewComponents |
| `tests/` | xUnit test projects mirror Application layer |
| Root `*.md` | Lifecycle and assessment artifacts |

## Verification checklist before submission

- [ ] `dotnet build` — 0 errors
- [ ] `dotnet test` — all passed
- [ ] App starts and login works
- [ ] All required root markdown files present
- [ ] `ai-prompts/` folder with phase files
- [ ] `candidate-info.md` filled in
- [ ] No secrets in committed files
