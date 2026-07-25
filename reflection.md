# Reflection

## What went well

**Clean Architecture paid off early.** Separating `QuotationCalculator` into the Application layer meant I could verify tax and discount math with unit tests before wiring up the full quotation create flow. When the first test failed, the fix was isolated to test expectations—not buried in a controller or database call.

**Settings-driven PDF branding** was the most satisfying cross-layer feature. Saving a primary color in Settings and seeing it appear in both the navigation accent and the next PDF download demonstrated that configuration flows correctly from UI → database → document generator.

**Seed data made manual testing fast.** Having realistic products, customers, and a company profile meant I could focus on workflow bugs instead of typing test data repeatedly.

## What was challenging

**CompanyId claim on login** caused confusing empty lists until I traced the issue to a missing claim on the identity principal. This taught me that ASP.NET Identity custom properties don't automatically become claims—you have to add them explicitly.

**Quotation line items in the UI** required client-side JavaScript to clone rows while the server re-validates everything. Keeping the form model binding correct across dynamic rows took iteration.

**Lifecycle documentation** was initially deferred in favor of code, which caused the structure gate failure on assessment. The code worked, but the submission scaffold was incomplete.

## What I would do differently

1. **Create lifecycle files first** — The assessment requires specific markdown artifacts at the root. I would scaffold all required files before writing code.
2. **Split prompt history by phase** — A single `prompt-history.md` is harder to evaluate than `ai-prompts/planning.md`, `implementation.md`, etc.
3. **Add one integration test** — A single `WebApplicationFactory` test for login would increase confidence without much effort.

## Skills demonstrated

- .NET 8 solution design with layered architecture
- EF Core migrations, seeding, and SQL Server persistence
- ASP.NET Identity with custom claims
- Server-side validation and consistent error handling
- PDF generation with external library (QuestPDF)
- Unit testing business logic with xUnit
- Technical documentation across the full SDLC

## Stretch goals (if continuing)

- Sales invoice conversion from quotation with stock decrement
- Chart.js dashboard charts
- Docker Compose for SQL Server + app
- GitHub Actions CI pipeline
- Integration and UI test suite

## AI usage honesty

AI (Cursor) accelerated scaffolding, boilerplate generation, and documentation drafting. I reviewed all generated code, fixed calculation test expectations manually, and debugged the CompanyId claim issue myself. See `final-ai-usage-summary.md` and `ai-prompts/` for details.
