# Code Review & Reflection

## Strengths

- **Clean Architecture** — Domain has no infrastructure dependencies; Web does not reference EF Core directly.
- **ServiceResult pattern** — Validation errors flow consistently from services to MVC ModelState.
- **Settings-driven PDFs** — Demonstrates configuration → document pipeline without hardcoded branding.
- **Focused scope** — Core features complete without stretch creep.

## Trade-offs

- **Single-tenant demo** — All users share company ID 1; production would need tenant resolution per user/org.
- **Synchronous PDF in request** — Acceptable for demo; large volumes would need background generation.
- **Client-side quotation lines** — JavaScript clones rows; server re-validates all inputs.

## Improvements (if continuing)

1. Integration tests with `WebApplicationFactory` for login and quotation POST.
2. `IClaimsPrincipalFactory` instead of manual claim refresh on login.
3. Stock decrement on quotation conversion (stretch — sales invoice).
4. CI workflow: build, test, optional migration smoke check.
5. Move seeded passwords to configuration for non-demo environments.

## Self-review checklist

- [x] No business logic in Razor views (display only)
- [x] No DbContext in controllers
- [x] Migrations checked in
- [x] README documents setup
- [x] Tests cover calculation and settings defaults
