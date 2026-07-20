using InventoryManagement.Application.DTOs;
using InventoryManagement.Shared.Constants;
using InventoryManagement.Shared.Theme;

namespace InventoryManagement.Application.Services;

public static class SettingsDefaults
{
    public const string DefaultInvoiceTerms =
        "1. Prices are valid until the date mentioned above.\n2. Payment terms: 50% advance, balance on delivery.\n3. Goods once sold will not be taken back.";

    public const string DefaultInvoiceFooter =
        "Thank you for your business. For queries contact our sales team.";

    public static CompanySettingsDto BuildSettingsDto(CompanyProfileDto profile, IDictionary<string, string> settings)
    {
        return new CompanySettingsDto
        {
            Profile = profile,
            PrimaryColor = ThemeHelper.NormalizePrimaryColor(
                GetSetting(settings, SettingKeys.PrimaryColor, ThemeHelper.DefaultPrimaryColor)),
            InvoiceTerms = GetSetting(settings, SettingKeys.InvoiceTerms, DefaultInvoiceTerms),
            InvoiceFooter = GetSetting(settings, SettingKeys.InvoiceFooter, DefaultInvoiceFooter)
        };
    }

    public static string GetSetting(IDictionary<string, string> settings, string key, string defaultValue)
    {
        return settings.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : defaultValue;
    }
}
