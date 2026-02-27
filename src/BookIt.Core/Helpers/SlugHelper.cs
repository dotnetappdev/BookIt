using System.Text.RegularExpressions;

namespace BookIt.Core.Helpers;

public static class SlugHelper
{
    /// <summary>
    /// Converts a string into a URL-friendly slug, e.g. "Men's Haircut!" → "mens-haircut"
    /// </summary>
    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Lowercase
        var slug = text.ToLowerInvariant();

        // Replace apostrophes/smart quotes without a separator
        slug = slug.Replace("'", "").Replace("\u2018", "").Replace("\u2019", "");

        // Replace non-alphanumeric characters with a hyphen
        slug = Regex.Replace(slug, @"[^a-z0-9]+", "-");

        // Trim leading/trailing hyphens and collapse multiple hyphens
        slug = slug.Trim('-');
        slug = Regex.Replace(slug, @"-{2,}", "-");

        return slug;
    }
}
