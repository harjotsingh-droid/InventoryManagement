using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Services;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Persistence;
using InventoryManagement.Shared.Constants;
using InventoryManagement.Shared.Theme;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Services;

public class CompanySettingsService : ICompanySettingsService
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CompanySettingsService(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ServiceResult<CompanySettingsDto>> GetSettingsAsync()
    {
        var company = await _db.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == _currentUser.CompanyId);

        if (company == null)
        {
            return ServiceResult<CompanySettingsDto>.Fail("Company not found.");
        }

        var settings = await _db.CompanySettings
            .AsNoTracking()
            .Where(s => s.CompanyId == _currentUser.CompanyId)
            .ToDictionaryAsync(s => s.Key, s => s.Value);

        var profile = MapProfile(company);
        var dto = SettingsDefaults.BuildSettingsDto(profile, settings);

        return ServiceResult<CompanySettingsDto>.Ok(dto);
    }

    public async Task<ServiceResult<CompanySettingsDto>> UpdateSettingsAsync(UpdateCompanySettingsDto dto)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            errors["Name"] = ["Company name is required."];
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            errors["Email"] = ["Email is required."];
        }

        if (errors.Count > 0)
        {
            return ServiceResult<CompanySettingsDto>.ValidationFail(errors);
        }

        var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == _currentUser.CompanyId);
        if (company == null)
        {
            return ServiceResult<CompanySettingsDto>.Fail("Company not found.");
        }

        company.Name = dto.Name.Trim();
        company.Address = dto.Address?.Trim();
        company.City = dto.City?.Trim();
        company.State = dto.State?.Trim();
        company.PinCode = dto.PinCode?.Trim();
        company.GstNumber = dto.GstNumber?.Trim();
        company.PanNumber = dto.PanNumber?.Trim();
        company.Mobile = dto.Mobile?.Trim();
        company.Email = dto.Email?.Trim();

        await UpsertSettingAsync(SettingKeys.PrimaryColor, ThemeHelper.NormalizePrimaryColor(dto.PrimaryColor));
        await UpsertSettingAsync(SettingKeys.InvoiceTerms, dto.InvoiceTerms?.Trim() ?? SettingsDefaults.DefaultInvoiceTerms);
        await UpsertSettingAsync(SettingKeys.InvoiceFooter, dto.InvoiceFooter?.Trim() ?? SettingsDefaults.DefaultInvoiceFooter);

        await _db.SaveChangesAsync();

        return await GetSettingsAsync();
    }

    private async Task UpsertSettingAsync(string key, string value)
    {
        var setting = await _db.CompanySettings
            .FirstOrDefaultAsync(s => s.CompanyId == _currentUser.CompanyId && s.Key == key);

        if (setting == null)
        {
            _db.CompanySettings.Add(new CompanySetting
            {
                CompanyId = _currentUser.CompanyId,
                Key = key,
                Value = value
            });
        }
        else
        {
            setting.Value = value;
        }
    }

    private static CompanyProfileDto MapProfile(Company company) => new()
    {
        Id = company.Id,
        Name = company.Name,
        Tagline = company.Tagline,
        Address = company.Address,
        City = company.City,
        State = company.State,
        Country = company.Country,
        PinCode = company.PinCode,
        GstNumber = company.GstNumber,
        PanNumber = company.PanNumber,
        Mobile = company.Mobile,
        Email = company.Email,
        Website = company.Website
    };
}
