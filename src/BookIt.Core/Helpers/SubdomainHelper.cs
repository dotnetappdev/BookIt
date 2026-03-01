namespace BookIt.Core.Helpers;

/// <summary>
/// Utility for extracting a tenant slug from a request host when wildcard subdomain routing is used.
/// For example, given host "demo-barber.bookit.com" and base domain "bookit.com", returns "demo-barber".
/// </summary>
public static class SubdomainHelper
{
    /// <summary>
    /// Extracts the tenant slug from a hostname using the configured base domain.
    /// Returns <c>null</c> if no valid single-level subdomain is found or if the subdomain is "www".
    /// </summary>
    /// <param name="host">The raw Host header value, e.g. "demo-barber.bookit.com" or "demo-barber.bookit.com:5000".</param>
    /// <param name="baseDomain">The base domain to strip, e.g. "bookit.com".</param>
    public static string? ExtractTenantSlug(string? host, string? baseDomain)
    {
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(baseDomain))
            return null;

        // Strip port number if present
        var hostWithoutPort = host.Contains(':') ? host[..host.LastIndexOf(':')] : host;

        hostWithoutPort = hostWithoutPort.ToLowerInvariant();
        var normalizedBase = baseDomain.ToLowerInvariant().TrimStart('.');

        // Host must end with ".<baseDomain>" to contain a subdomain
        if (!hostWithoutPort.EndsWith("." + normalizedBase, StringComparison.Ordinal))
            return null;

        var subdomain = hostWithoutPort[..^(normalizedBase.Length + 1)];

        // Ignore empty, "www", or nested subdomains (e.g. "a.b")
        if (string.IsNullOrWhiteSpace(subdomain) || subdomain == "www" || subdomain.Contains('.'))
            return null;

        return subdomain;
    }
}
