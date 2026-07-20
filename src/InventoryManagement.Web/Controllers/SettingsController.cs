using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers;

[Authorize]
public class SettingsController : Controller
{
    private readonly ICompanySettingsService _settingsService;

    public SettingsController(ICompanySettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _settingsService.GetSettingsAsync();
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.ErrorMessage ?? "Unable to load settings.";
            return View(new SettingsFormViewModel());
        }

        var settings = result.Data;
        return View(new SettingsFormViewModel
        {
            Name = settings.Profile.Name,
            Address = settings.Profile.Address,
            City = settings.Profile.City,
            State = settings.Profile.State,
            PinCode = settings.Profile.PinCode,
            GstNumber = settings.Profile.GstNumber,
            PanNumber = settings.Profile.PanNumber,
            Mobile = settings.Profile.Mobile,
            Email = settings.Profile.Email ?? string.Empty,
            PrimaryColor = settings.PrimaryColor,
            InvoiceTerms = settings.InvoiceTerms,
            InvoiceFooter = settings.InvoiceFooter
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SettingsFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _settingsService.UpdateSettingsAsync(new UpdateCompanySettingsDto
        {
            Name = model.Name,
            Address = model.Address,
            City = model.City,
            State = model.State,
            PinCode = model.PinCode,
            GstNumber = model.GstNumber,
            PanNumber = model.PanNumber,
            Mobile = model.Mobile,
            Email = model.Email,
            PrimaryColor = model.PrimaryColor,
            InvoiceTerms = model.InvoiceTerms,
            InvoiceFooter = model.InvoiceFooter
        });

        if (!result.Success)
        {
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            if (result.ValidationErrors != null)
            {
                foreach (var error in result.ValidationErrors)
                {
                    ModelState.AddModelError(error.Key, string.Join(" ", error.Value));
                }
            }

            return View(model);
        }

        TempData["Success"] = "Settings saved. The next quotation PDF will use the updated branding.";
        return RedirectToAction(nameof(Index));
    }
}
