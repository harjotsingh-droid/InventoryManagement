# AI Prompts — Design Phase

## Prompt 1 — Architecture design

**Date:** 2026-07-01

**Prompt:**
> Design a Clean Architecture solution for SME ERP Inventory Management with .NET 8. Projects: Domain, Application, Infrastructure, Shared, Web, and Tests. Controllers must not reference DbContext. Use ServiceResult for errors.

**Outcome:**
- Solution structure with dependency direction
- `ServiceResult<T>` pattern adopted
- Documented in `design-notes.md`

## Prompt 2 — Data model

**Date:** 2026-07-01

**Prompt:**
> Design entities for Company, CompanySetting (key-value), Product, Customer, Quotation, QuotationLine with CompanyId for multi-tenancy. Include calculation rules for Indian GST (discount before tax).

**Outcome:**
- Entity definitions with relationships
- Quotation calculation rules documented
- Saved to `data-model.md` with ER diagram

## Prompt 3 — API / route contract

**Date:** 2026-07-02

**Prompt:**
> Document the MVC route contract for all controllers: Account, Home, Products, Customers, Quotations, Settings, Search. Include HTTP methods, auth requirements, and service interface methods.

**Outcome:**
- Route table per controller
- Service method signatures
- Saved to `api-contract.md`

## Prompt 4 — UI flows

**Date:** 2026-07-02

**Prompt:**
> Document user flows for login, product CRUD, quotation creation with line items, settings affecting PDF branding, global search, and dashboard. Include error handling patterns.

**Outcome:**
- Step-by-step flows with navigation diagram
- Error handling table
- Saved to `ui-flow.md`
