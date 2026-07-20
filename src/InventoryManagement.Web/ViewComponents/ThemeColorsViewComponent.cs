using InventoryManagement.Application.Interfaces;
using InventoryManagement.Shared.Theme;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.ViewComponents;

public class ThemeColorsViewComponent : ViewComponent
{
    private readonly ICompanySettingsService _settingsService;

    public ThemeColorsViewComponent(ICompanySettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (HttpContext.User.Identity?.IsAuthenticated != true)
        {
            return View("Default", ThemeHelper.DefaultPrimaryColor);
        }

        var result = await _settingsService.GetSettingsAsync();
        var color = result.Data?.PrimaryColor ?? ThemeHelper.DefaultPrimaryColor;
        return View("Default", color);
    }
}
