# Acceptance Criteria

## Authentication

| ID | Criterion | Status |
|----|-----------|--------|
| AC-01 | User can log in with seeded credentials | ✅ |
| AC-02 | Invalid credentials show an error message | ✅ |
| AC-03 | User can log out and is redirected to login | ✅ |
| AC-04 | Protected pages require authentication | ✅ |

## Products

| ID | Criterion | Status |
|----|-----------|--------|
| AC-05 | Product list displays seeded products | ✅ |
| AC-06 | Product search filters by keyword | ✅ |
| AC-07 | User can create a new product | ✅ |
| AC-08 | User can edit an existing product | ✅ |
| AC-09 | Duplicate SKU is rejected with validation error | ✅ |

## Customers

| ID | Criterion | Status |
|----|-----------|--------|
| AC-10 | Customer list displays seeded customers | ✅ |
| AC-11 | User can create a new customer | ✅ |
| AC-12 | User can edit an existing customer | ✅ |
| AC-13 | Required fields are validated | ✅ |

## Quotations

| ID | Criterion | Status |
|----|-----------|--------|
| AC-14 | User can create a quotation with multiple line items | ✅ |
| AC-15 | Quotation list shows all quotations for the company | ✅ |
| AC-16 | Quotation detail shows line items and totals | ✅ |
| AC-17 | User can download quotation as PDF | ✅ |
| AC-18 | Missing customer or zero quantity is rejected | ✅ |
| AC-19 | Line totals use discount-then-GST calculation | ✅ |

## Settings and PDF branding

| ID | Criterion | Status |
|----|-----------|--------|
| AC-20 | User can update company profile and invoice settings | ✅ |
| AC-21 | Primary color setting drives UI theme accent | ✅ |
| AC-22 | PDF reflects updated company address and color | ✅ |
| AC-23 | Invoice terms and footer appear on generated PDF | ✅ |

## Global search and dashboard

| ID | Criterion | Status |
|----|-----------|--------|
| AC-24 | Global search finds products, customers, and quotations | ✅ |
| AC-25 | Search requires minimum 2 characters | ✅ |
| AC-26 | Dashboard shows live KPI counts | ✅ |

## Technical

| ID | Criterion | Status |
|----|-----------|--------|
| AC-27 | Data persists in SQL Server via EF Core migrations | ✅ |
| AC-28 | Multi-tenant isolation via CompanyId on entities | ✅ |
| AC-29 | Controllers do not reference DbContext directly | ✅ |
| AC-30 | No secrets committed to repository | ✅ |
| AC-31 | README documents setup and run instructions | ✅ |
| AC-32 | At least 2 xUnit tests pass (`dotnet test`) | ✅ (5 tests) |

## Out of scope (stretch — not required)

- POS billing, purchases, suppliers, payments, HR
- User management UI, Chart.js charts, stock audit
- Docker/CI pipeline, integration tests
