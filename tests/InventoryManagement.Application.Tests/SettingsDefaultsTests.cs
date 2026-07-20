using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Services;
using InventoryManagement.Shared.Constants;
using InventoryManagement.Shared.Theme;

namespace InventoryManagement.Application.Tests;

public class SettingsDefaultsTests
{
    [Fact]
    public void BuildSettingsDto_WithMissingSettings_UsesDefaults()
    {
        var profile = new CompanyProfileDto
        {
            Id = 1,
            Name = "Test Co.",
            GstNumber = "GST123",
            Email = "test@example.com"
        };

        var settings = new Dictionary<string, string>();

        var dto = SettingsDefaults.BuildSettingsDto(profile, settings);

        Assert.Equal(ThemeHelper.DefaultPrimaryColor, dto.PrimaryColor);
        Assert.Equal(SettingsDefaults.DefaultInvoiceTerms, dto.InvoiceTerms);
        Assert.Equal(SettingsDefaults.DefaultInvoiceFooter, dto.InvoiceFooter);
        Assert.Equal("Test Co.", dto.Profile.Name);
    }

    [Fact]
    public void BuildSettingsDto_WithStoredSettings_UsesConfiguredValues()
    {
        var profile = new CompanyProfileDto { Name = "Branded Co." };
        var settings = new Dictionary<string, string>
        {
            [SettingKeys.PrimaryColor] = "#ff0000",
            [SettingKeys.InvoiceTerms] = "Custom terms apply.",
            [SettingKeys.InvoiceFooter] = "Custom footer."
        };

        var dto = SettingsDefaults.BuildSettingsDto(profile, settings);

        Assert.Equal("#ff0000", dto.PrimaryColor);
        Assert.Equal("Custom terms apply.", dto.InvoiceTerms);
        Assert.Equal("Custom footer.", dto.InvoiceFooter);
    }

    [Fact]
    public void GetSetting_WhenKeyMissing_ReturnsDefault()
    {
        var value = SettingsDefaults.GetSetting(new Dictionary<string, string>(), SettingKeys.InvoiceTerms, "fallback");

        Assert.Equal("fallback", value);
    }
}
