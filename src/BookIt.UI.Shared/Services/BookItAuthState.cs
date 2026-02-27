using BookIt.Core.DTOs;
using BookIt.Core.Enums;

namespace BookIt.UI.Shared.Services;

/// <summary>
/// Holds the current user's authentication state.
/// Scoped service — one instance per Blazor circuit / MAUI session.
/// </summary>
public class BookItAuthState
{
    public string? AccessToken { get; private set; }
    public string? TenantSlug { get; private set; }
    public string? UserName { get; private set; }
    public string? UserEmail { get; private set; }
    public UserRole? Role { get; private set; }
    public Guid? TenantId { get; private set; }
    public string? MembershipNumber { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);

    public bool IsManagerOrAbove => Role is UserRole.TenantAdmin or UserRole.SuperAdmin or UserRole.Manager;
    public bool IsStaffOrAbove => Role is UserRole.Staff or UserRole.Manager or UserRole.TenantAdmin or UserRole.SuperAdmin;
    public bool IsStaffOnly => Role == UserRole.Staff;

    public string Initials
    {
        get
        {
            if (string.IsNullOrEmpty(UserName)) return "?";
            var parts = UserName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(parts.Take(2).Select(p => char.ToUpper(p[0]))).TrimEnd();
        }
    }

    public event Action? OnChange;

    public void SetUser(AuthResponse auth)
    {
        AccessToken = auth.AccessToken;
        TenantSlug = auth.TenantSlug;
        UserName = auth.FullName;
        UserEmail = auth.Email;
        Role = auth.Role;
        TenantId = auth.TenantId;
        MembershipNumber = auth.MembershipNumber;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        AccessToken = null;
        TenantSlug = null;
        UserName = null;
        UserEmail = null;
        Role = null;
        TenantId = null;
        MembershipNumber = null;
        OnChange?.Invoke();
    }
}
