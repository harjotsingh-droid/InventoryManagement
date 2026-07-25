# Cursor Prompt Conventions

Reusable prompt patterns used during this project.

## Planning prompts

```
Help me define trimmed core scope vs stretch for Option 3 SME ERP Inventory Management.
Include: auth, products, customers, quotations with PDF, settings, search, dashboard, EF Core, tests.
Output as markdown table mapping requirement → implementation.
```

## Design prompts

```
Design Clean Architecture for .NET 8 with Domain, Application, Infrastructure, Web, Tests.
Controllers must not reference DbContext. Use ServiceResult<T> for errors.
Include CompanyId multi-tenancy on all business entities.
```

## Implementation prompts

```
Create [Controller/Service] for [feature] following existing patterns in [file path].
Use ServiceResult<T>, filter by CompanyId via ICurrentUserService, validate on server.
```

## Testing prompts

```
Create xUnit tests for [class] with known input values.
Test [specific scenario]. Use Arrange-Act-Assert pattern.
```

## Debugging prompts

```
[Describe symptom]. Here's the relevant code: [paste].
Suggest root cause and fix following Clean Architecture constraints.
```

## Code review prompts

```
Review [solution area] for architecture violations, security issues, missing validations.
Output findings table with severity (Good / Low / Medium / High).
```

## Documentation prompts

```
Create [artifact name] for assessment submission based on the existing codebase.
Include [specific sections]. Match content to actual implementation, not placeholders.
```

## Anti-patterns to avoid

| Bad prompt | Better prompt |
|------------|---------------|
| "Build the whole app" | "Scaffold Domain entities for Company, Product, Customer, Quotation" |
| "Fix the bug" | "Login works but product list is empty; CompanyId claim may be missing" |
| "Write docs" | "Create data-model.md with ER diagram matching entities in Domain/" |
