# Test Strategy

## Objectives

1. Verify quotation calculation logic is correct (discount-then-GST).
2. Verify settings defaults fallback when keys are missing.
3. Support manual verification of end-to-end workflows.
4. Record automated test output for assessment evidence.

## Test levels

### Unit tests (automated)

**Framework:** xUnit (.NET 8)
**Location:** `tests/InventoryManagement.Application.Tests/`

| Test class | What it validates | Why unit level |
|------------|-------------------|----------------|
| `QuotationCalculatorTests` | Line and document totals with known inputs | Pure logic, no I/O |
| `SettingsDefaultsTests` | Default and override behavior for company settings | Pure logic, no I/O |

**Run command:**
```powershell
dotnet test
```

**Coverage focus:** Business rules that would be expensive to verify manually and prone to regression.

### Manual tests (checklist)

**Location:** `testing-notes.md`

Covers authentication, CRUD flows, PDF branding, global search, and persistence after restart. Used because core scope does not require UI/integration automation.

### Integration tests (deferred)

Not in trimmed core scope. Recommended follow-up:
- `WebApplicationFactory` for login POST
- Quotation create POST with in-memory or test database

## Test data

- Seeded via `DbSeeder` on first startup
- Demo users: `admin@demo.com`, `user@demo.com`
- 6 products, 4 customers, 1 company with 3 settings

## Pass/fail criteria

| Check | Pass condition |
|-------|----------------|
| `dotnet test` | 0 failed, all tests passed |
| `dotnet build` | 0 errors |
| Manual checklist | Critical paths verified (login, quotation, PDF, settings) |

## Known gaps

- No automated UI tests (Selenium/Playwright)
- No service-layer tests with mocked `DbContext`
- No PDF content assertion tests

## Test execution record

See `test-results.md` for captured `dotnet test` output (5 tests passed, 2026-07-02).
