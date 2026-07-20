using System.Security.Claims;
using InventoryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int CompanyId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("CompanyId");
            return int.TryParse(claim, out var companyId) ? companyId : 0;
        }
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
}
