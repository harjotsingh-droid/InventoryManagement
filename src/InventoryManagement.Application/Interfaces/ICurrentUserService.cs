using InventoryManagement.Application.Common;
using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Interfaces;

public interface ICurrentUserService
{
    int CompanyId { get; }
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}
