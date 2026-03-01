using BookIt.Core.Helpers;

namespace BookIt.Tests.Domain;

public class SubdomainHelperTests
{
    [Theory]
    [InlineData("demo-barber.bookit.com", "bookit.com", "demo-barber")]
    [InlineData("my-salon.example.co.uk", "example.co.uk", "my-salon")]
    [InlineData("TENANT.BookIt.com", "bookit.com", "tenant")]  // case-insensitive
    [InlineData("demo-barber.bookit.com:5000", "bookit.com", "demo-barber")]  // with port
    public void ExtractTenantSlug_ReturnsSlug_WhenSubdomainPresent(string host, string baseDomain, string expectedSlug)
    {
        var result = SubdomainHelper.ExtractTenantSlug(host, baseDomain);
        Assert.Equal(expectedSlug, result);
    }

    [Theory]
    [InlineData("bookit.com", "bookit.com")]            // no subdomain
    [InlineData("www.bookit.com", "bookit.com")]        // www is ignored
    [InlineData("otherdomain.com", "bookit.com")]       // different domain
    [InlineData("localhost", "bookit.com")]              // localhost
    [InlineData("a.b.bookit.com", "bookit.com")]        // nested subdomain
    [InlineData("bookit.com:5000", "bookit.com")]       // base domain with port, no sub
    public void ExtractTenantSlug_ReturnsNull_WhenNoValidSubdomain(string host, string baseDomain)
    {
        var result = SubdomainHelper.ExtractTenantSlug(host, baseDomain);
        Assert.Null(result);
    }

    [Theory]
    [InlineData(null, "bookit.com")]
    [InlineData("demo-barber.bookit.com", null)]
    [InlineData("", "bookit.com")]
    [InlineData("demo-barber.bookit.com", "")]
    public void ExtractTenantSlug_ReturnsNull_WhenNullOrEmptyInput(string? host, string? baseDomain)
    {
        var result = SubdomainHelper.ExtractTenantSlug(host, baseDomain);
        Assert.Null(result);
    }
}
