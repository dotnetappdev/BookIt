using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(BookItDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        ApplicationUser? user;
        Tenant? tenant;

        if (!string.IsNullOrEmpty(request.TenantSlug))
        {
            tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Slug == request.TenantSlug && !t.IsDeleted);

            if (tenant == null)
                return Unauthorized(new { message = "Invalid tenant" });

            user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.TenantId == tenant.Id && u.Email == request.Email && !u.IsDeleted);
        }
        else
        {
            user = await _userManager.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

            if (user?.Tenant == null)
                return Unauthorized(new { message = "Invalid credentials" });

            tenant = user.Tenant;
        }

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Unauthorized(new { message = "Invalid credentials" });

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, tenant, roles);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new AuthResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Role = user.Role,
            TenantId = tenant.Id,
            TenantSlug = tenant.Slug,
            MembershipNumber = user.MembershipNumber
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        Tenant? tenant;

        if (!string.IsNullOrEmpty(request.TenantSlug))
        {
            tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Slug == request.TenantSlug && !t.IsDeleted);

            if (tenant == null)
                return BadRequest(new { message = "Invalid tenant" });
        }
        else
        {
            var existingUser = await _userManager.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

            if (existingUser?.Tenant == null)
                return BadRequest(new { message = "User account not found. Please contact your organization administrator." });

            tenant = existingUser.Tenant;
        }

        var existingUserCheck = await _userManager.FindByEmailAsync(request.Email);
        if (existingUserCheck != null && existingUserCheck.TenantId == tenant.Id)
            return Conflict(new { message = "Email already registered" });

        var user = new ApplicationUser
        {
            TenantId = tenant.Id,
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.Phone,
            MembershipNumber = request.MembershipNumber,
            Role = UserRole.Customer,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

        await _userManager.AddToRoleAsync(user, "Customer");

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, tenant, roles);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return CreatedAtAction(nameof(Login), new AuthResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Role = user.Role,
            TenantId = tenant.Id,
            TenantSlug = tenant.Slug,
            MembershipNumber = user.MembershipNumber
        });
    }

    [HttpPost("setup")]
    public async Task<ActionResult<AuthResponse>> Setup([FromBody] TenantSetupRequest request)
    {
        var slug = request.BusinessName.ToLower().Replace(" ", "-").Trim();

        var exists = await _context.Tenants.AnyAsync(t => t.Slug == slug);
        if (exists)
            return Conflict(new { message = "A tenant with this name already exists" });

        var tenant = new Tenant
        {
            Name = request.BusinessName,
            Slug = slug,
            BusinessType = request.BusinessType,
            ContactEmail = request.AdminEmail,
            ContactPhone = request.Phone,
            Address = request.Address,
            TimeZone = request.TimeZone ?? "UTC",
            Currency = request.Currency ?? "GBP",
            IsActive = true
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        var user = new ApplicationUser
        {
            TenantId = tenant.Id,
            Email = request.AdminEmail,
            UserName = request.AdminEmail,
            FirstName = request.AdminFirstName,
            LastName = request.AdminLastName,
            Role = UserRole.TenantAdmin,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.AdminPassword);
        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

        await _userManager.AddToRoleAsync(user, "TenantAdmin");

        // Default business hours Mon-Fri
        for (int day = 1; day <= 5; day++)
        {
            _context.BusinessHours.Add(new Core.Entities.BusinessHours
            {
                TenantId = tenant.Id,
                DayOfWeek = (DayOfWeekFlag)day,
                OpenTime = new TimeOnly(9, 0),
                CloseTime = new TimeOnly(17, 0),
                SlotDurationMinutes = 60,
                IsClosed = false
            });
        }

        _context.BookingForms.Add(new BookingForm
        {
            TenantId = tenant.Id,
            Name = "Default Booking Form",
            IsDefault = true,
            IsActive = true,
            WelcomeMessage = $"Welcome to {request.BusinessName}",
            ConfirmationMessage = "Your appointment has been confirmed!"
        });

        if (!string.IsNullOrEmpty(request.ConnectionString))
        {
            _context.AppConfigurations.Add(new AppConfiguration
            {
                TenantId = tenant.Id,
                Key = "ConnectionString",
                Value = request.ConnectionString,
                IsEncrypted = true
            });
        }

        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, tenant, roles);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return CreatedAtAction(nameof(Login), new AuthResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Role = user.Role,
            TenantId = tenant.Id,
            TenantSlug = tenant.Slug
        });
    }

    private string GenerateJwtToken(ApplicationUser user, Tenant tenant, IList<string> roles)
    {
        var key = _configuration["Jwt:Key"] ?? "BookItSuperSecretKey-ChangeInProduction-Must-Be-At-Least-32-Characters!";
        var issuer = _configuration["Jwt:Issuer"] ?? "BookIt.API";
        var audience = _configuration["Jwt:Audience"] ?? "BookIt.Web";
        var expiryHours = int.Parse(_configuration["Jwt:ExpiryHours"] ?? "24");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("tenant_id", tenant.Id.ToString()),
            new Claim("tenant_slug", tenant.Slug),
            new Claim("role", ((int)user.Role).ToString()),
            new Claim("given_name", user.FirstName),
            new Claim("family_name", user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
