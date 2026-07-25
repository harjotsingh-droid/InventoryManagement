# Code Review Notes

AI-assisted and self-review of the SME ERP Inventory Management implementation.

## Architecture review

| Area | Finding | Severity |
|------|---------|----------|
| Layer separation | Controllers depend only on application interfaces; no `DbContext` in Web layer | ✅ Good |
| Domain purity | Domain project has no infrastructure references | ✅ Good |
| DI registration | All services registered in `Infrastructure/DependencyInjection.cs` | ✅ Good |
| Multi-tenancy | All queries filter by `CompanyId` | ✅ Good |

## Security review

| Area | Finding | Severity |
|------|---------|----------|
| Authentication | `[Authorize]` on all business controllers | ✅ Good |
| Secrets | Connection string uses placeholder; user secrets documented | ✅ Good |
| Seeded passwords | Demo-only; documented in README | ⚠️ Low (acceptable for demo) |
| CSRF | POST actions use anti-forgery tokens (MVC default) | ✅ Good |

## Business logic review

| Area | Finding | Severity |
|------|---------|----------|
| Quotation math | Centralized in `QuotationCalculator`; unit tested | ✅ Good |
| Validation | Server-side validation in services; client JS is convenience only | ✅ Good |
| SKU uniqueness | Checked in `ProductService` before insert/update | ✅ Good |
| PDF branding | Reads live settings, not cached hardcoded values | ✅ Good |

## Code quality observations

**Strengths:**
- Consistent `ServiceResult<T>` error handling across controllers
- DTOs separate API shape from EF entities
- Seed data provides realistic demo scenario
- README and database notes support reproducibility

**Issues found during review:**

1. **CompanyId claim dependency** — Services return empty results if claim missing. Mitigated by `RefreshClaimsAsync` on login.
2. **Synchronous PDF** — Blocks request thread; acceptable for demo, not for high volume.
3. **No integration tests** — Login and quotation POST flows untested automatically.
4. **Theme refresh** — Layout color may need page refresh after settings save.

## Suggested improvements (prioritized)

1. Add `IClaimsPrincipalFactory` for automatic `CompanyId` claim
2. Add integration test for quotation create workflow
3. Add CI workflow: `dotnet build` + `dotnet test`
4. Extract quotation line JavaScript to separate file for maintainability

## Review method

- Self-review against acceptance criteria
- AI-assisted review via Cursor for layer violations and missing validations
- Manual smoke test per `testing-notes.md` checklist
