# Test Results

Recorded: 2026-07-02

## Command

```powershell
dotnet test
```

## Output

```
Test run for ... InventoryManagement.Application.Tests.dll (.NETCoreApp,Version=v8.0)
VSTest version 17.14.1 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:     5, Skipped:     0, Total:     5, Duration: 12 ms
```

## Tests executed

| Test | Result |
|------|--------|
| `QuotationCalculatorTests.Calculate_WithKnownLineItems_ProducesCorrectTotals` | Passed |
| `QuotationCalculatorTests.CalculateLine_WithDiscountAndGst_ComputesLineTotals` | Passed |
| `SettingsDefaultsTests.BuildSettingsDto_WithMissingSettings_UsesDefaults` | Passed |
| `SettingsDefaultsTests.BuildSettingsDto_WithStoredSettings_UsesConfiguredValues` | Passed |
| `SettingsDefaultsTests.GetSetting_WhenKeyMissing_ReturnsDefault` | Passed |

## Build

```powershell
dotnet build
```

Build succeeded — 0 Warning(s), 0 Error(s) (after decimal precision configuration).
