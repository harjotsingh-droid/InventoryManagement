# Test Results

Recorded: 2026-07-25

## Command

```powershell
dotnet test
```

## Output

```
Test run for ... InventoryManagement.Application.Tests.dll (.NETCoreApp,Version=v8.0)
Passed!  - Failed: 0, Passed: 7, Skipped: 0, Total: 7

Test run for ... InventoryManagement.Web.Tests.dll (.NETCoreApp,Version=v8.0)
Passed!  - Failed: 0, Passed: 5, Skipped: 0, Total: 5
```

## Unit tests (Application.Tests)

| Test | Result |
|------|--------|
| `QuotationCalculatorTests.Calculate_WithKnownLineItems_ProducesCorrectTotals` | Passed |
| `QuotationCalculatorTests.CalculateLine_WithDiscountAndGst_ComputesLineTotals` | Passed |
| `QuotationCalculatorTests.CalculateLine_WithFullDiscount_HasZeroTax` | Passed |
| `QuotationCalculatorTests.Calculate_WithQuotationDiscount_SubtractsFromDocumentTotal` | Passed |
| `SettingsDefaultsTests.BuildSettingsDto_WithMissingSettings_UsesDefaults` | Passed |
| `SettingsDefaultsTests.BuildSettingsDto_WithStoredSettings_UsesConfiguredValues` | Passed |
| `SettingsDefaultsTests.GetSetting_WhenKeyMissing_ReturnsDefault` | Passed |

## Integration tests (Web.Tests)

| Test | Result |
|------|--------|
| `QuotationsIntegrationTests.Login_WithSeededCredentials_RedirectsToDashboard` | Passed |
| `QuotationsIntegrationTests.CreateQuotation_AfterLogin_PersistsAndRedirectsToDetails` | Passed |
| `QuotationsIntegrationTests.CreateQuotation_WithZeroQuantity_ReturnsValidationError` | Passed |
| `QuotationPdfGeneratorTests.Generate_WithDifferentCompanyNames_ProducesDifferentPdfOutput` | Passed |
| `QuotationPdfGeneratorTests.Generate_WithCustomTerms_ProducesValidPdfDocument` | Passed |

## Build

```powershell
dotnet build
```

Build succeeded — 0 Warning(s), 0 Error(s).
