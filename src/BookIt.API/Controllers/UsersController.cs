using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "SuperAdmin")]
public class UsersController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UsersController> _logger;

    public UsersController(BookItDbContext context, UserManager<ApplicationUser> userManager, ILogger<UsersController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>Lists all application users across all tenants.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserListResponse>>> GetAllUsers(
        [FromQuery] string? search = null,
        [FromQuery] UserRole? role = null,
        [FromQuery] Guid? tenantId = null)
    {
        var query = _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Tenant)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLowerInvariant();
            query = query.Where(u =>
                u.Email!.ToLower().Contains(s) ||
                u.FirstName.ToLower().Contains(s) ||
                u.LastName.ToLower().Contains(s));
        }

        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);

        if (tenantId.HasValue)
            query = query.Where(u => u.TenantId == tenantId.Value);

        var users = await query
            .OrderBy(u => u.CreatedAt)
            .Select(u => new UserListResponse
            {
                Id = u.Id,
                Email = u.Email ?? "",
                FullName = u.FirstName + " " + u.LastName,
                Role = u.Role,
                RoleName = u.Role.ToString(),
                TenantId = u.TenantId,
                TenantName = u.Tenant != null ? u.Tenant.Name : "",
                TenantSlug = u.Tenant != null ? u.Tenant.Slug : "",
                IsDeleted = u.IsDeleted,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    /// <summary>Updates the role of an application user.</summary>
    [HttpPut("{userId}/role")]
    public async Task<ActionResult<UserListResponse>> UpdateUserRole(Guid userId, [FromBody] UpdateUserRoleRequest request)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound(new { message = "User not found" });

        // Prevent demoting the last SuperAdmin
        if (user.Role == UserRole.SuperAdmin && request.Role != UserRole.SuperAdmin)
        {
            var superAdminCount = await _context.Users
                .IgnoreQueryFilters()
                .CountAsync(u => u.Role == UserRole.SuperAdmin && !u.IsDeleted && u.Id != userId);
            if (superAdminCount == 0)
                return BadRequest(new { message = "Cannot remove the last super admin." });
        }

        var oldRole = user.Role.ToString();
        user.Role = request.Role;

        var oldRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, oldRoles);
        await _userManager.AddToRoleAsync(user, request.Role.ToString());
        await _context.SaveChangesAsync();

        _logger.LogInformation("SuperAdmin changed user {UserId} role from {OldRole} to {NewRole}", userId, oldRole, request.Role);

        return Ok(new UserListResponse
        {
            Id = user.Id,
            Email = user.Email ?? "",
            FullName = user.FirstName + " " + user.LastName,
            Role = user.Role,
            RoleName = user.Role.ToString(),
            TenantId = user.TenantId,
            TenantName = user.Tenant?.Name ?? "",
            TenantSlug = user.Tenant?.Slug ?? "",
            IsDeleted = user.IsDeleted,
            CreatedAt = user.CreatedAt
        });
    }

    /// <summary>Soft-deletes (deactivates) an application user.</summary>
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound(new { message = "User not found" });

        if (user.Role == UserRole.SuperAdmin)
        {
            var superAdminCount = await _context.Users
                .IgnoreQueryFilters()
                .CountAsync(u => u.Role == UserRole.SuperAdmin && !u.IsDeleted && u.Id != userId);
            if (superAdminCount == 0)
                return BadRequest(new { message = "Cannot delete the last super admin." });
        }

        user.IsDeleted = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("SuperAdmin soft-deleted user {UserId} ({Email})", userId, user.Email);
        return NoContent();
    }
}
