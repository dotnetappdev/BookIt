using BookIt.Core.Enums;
using BookIt.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookIt.Tests.Domain;

public class TenantServiceTests
{
    [Fact]
    public void GetCurrentTenantId_ReturnsNull_WhenNoClaim()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new TenantService(accessor);

        // Act
        var tenantId = service.GetCurrentTenantId();

        // Assert
        Assert.Null(tenantId);
    }

    [Fact]
    public void GetCurrentTenantId_ReturnsTenantId_WhenClaimPresent()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var claims = new[] { new Claim("tenant_id", expectedId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new TenantService(accessor);

        // Act
        var tenantId = service.GetCurrentTenantId();

        // Assert
        Assert.Equal(expectedId, tenantId);
    }

    [Fact]
    public void IsValidTenantAccess_ReturnsFalse_WhenDifferentTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var differentTenantId = Guid.NewGuid();
        var claims = new[] { new Claim("tenant_id", tenantId.ToString()), new Claim("role", "2") };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new TenantService(accessor);

        // Act
        var result = service.IsValidTenantAccess(differentTenantId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidTenantAccess_ReturnsTrue_WhenSameTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var claims = new[] { new Claim("tenant_id", tenantId.ToString()), new Claim("role", "2") };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new TenantService(accessor);

        // Act
        var result = service.IsValidTenantAccess(tenantId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidTenantAccess_ReturnsTrue_ForSuperAdmin()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var differentTenantId = Guid.NewGuid();
        var claims = new[]
        {
            new Claim("tenant_id", tenantId.ToString()),
            new Claim("role", ((int)UserRole.SuperAdmin).ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new TenantService(accessor);

        // Act
        var result = service.IsValidTenantAccess(differentTenantId);

        // Assert
        Assert.True(result);
    }
}
