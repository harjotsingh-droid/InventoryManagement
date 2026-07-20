# Debugging Notes

## Issues and resolutions

### PowerShell command chaining

**Symptom:** `&&` not valid in older PowerShell when scaffolding projects.

**Fix:** Use semicolon-separated commands or separate invocations.

### CompanyId claim on login

**Symptom:** Services filter by `CompanyId = 0` if claim missing.

**Fix:** `AccountController.RefreshClaimsAsync` adds `CompanyId` claim after successful password sign-in and re-signs the user.

### Quotation total test expectations

**Symptom:** Initial unit test expected incorrect subtotal.

**Fix:** Recalculated line math: `(qty × price − line discount) + GST per line`, then subtract quotation-level discount.

### ViewComponent in CSS variable

**Symptom:** Primary color not applied in layout.

**Fix:** `ThemeColorsViewComponent` returns normalized color; `_Layout.cshtml` injects via `@await Component.InvokeAsync("ThemeColors")`.

## Useful commands

```powershell
dotnet build
dotnet test
dotnet ef database update --project src/InventoryManagement.Infrastructure --startup-project src/InventoryManagement.Web
dotnet run --project src/InventoryManagement.Web
```

## EF Core logging

Enable in `appsettings.Development.json`:

```json
"Logging": {
  "LogLevel": {
    "Microsoft.EntityFrameworkCore.Database.Command": "Information"
  }
}
```
