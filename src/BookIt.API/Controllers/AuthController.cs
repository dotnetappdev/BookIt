using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BookItDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == request.TenantSlug && !t.IsDeleted);

        if (tenant == null)
            return Unauthorized(new { message = "Invalid tenant" });

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == tenant.Id && u.Email == request.Email && !u.IsDeleted);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });

        var token = GenerateJwtToken(user, tenant);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            TenantId = tenant.Id,
            TenantSlug = tenant.Slug
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == request.TenantSlug && !t.IsDeleted);

        if (tenant == null)
            return BadRequest(new { message = "Invalid tenant" });

        var exists = await _context.Users
            .AnyAsync(u => u.TenantId == tenant.Id && u.Email == request.Email);

        if (exists)
            return Conflict(new { message = "Email already registered" });

        var user = new ApplicationUser
        {
            TenantId = tenant.Id,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Role = UserRole.Customer,
            IsEmailVerified = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user, tenant);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Login), new AuthResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            TenantId = tenant.Id,
            TenantSlug = tenant.Slug
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

        var user = new ApplicationUser
        {
            TenantId = tenant.Id,
            Email = request.AdminEmail,
            PasswordHash = HashPassword(request.AdminPassword),
            FirstName = request.AdminFirstName,
            LastName = request.AdminLastName,
            Role = UserRole.TenantAdmin,
            IsEmailVerified = true
        };

        _context.Users.Add(user);

        // Create default business hours (Mon-Fri)
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

        // Create default booking form
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

        var token = GenerateJwtToken(user, tenant);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Login), new AuthResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            TenantId = tenant.Id,
            TenantSlug = tenant.Slug
        });
    }

    private string GenerateJwtToken(ApplicationUser user, Tenant tenant)
    {
        var key = _configuration["Jwt:Key"] ?? "BookItSuperSecretKey-ChangeInProduction-Must-Be-At-Least-32-Characters!";
        var issuer = _configuration["Jwt:Issuer"] ?? "BookIt.API";
        var audience = _configuration["Jwt:Audience"] ?? "BookIt.Web";
        var expiryHours = int.Parse(_configuration["Jwt:ExpiryHours"] ?? "24");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("tenant_id", tenant.Id.ToString()),
            new Claim("tenant_slug", tenant.Slug),
            new Claim("role", ((int)user.Role).ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

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

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "BookItSalt"));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string hash)
        => HashPassword(password) == hash;

    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
