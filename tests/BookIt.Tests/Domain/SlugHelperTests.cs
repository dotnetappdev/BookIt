using BookIt.Core.Helpers;

namespace BookIt.Tests.Domain;

public class SlugHelperTests
{
    [Theory]
    [InlineData("Mens Haircut", "mens-haircut")]
    [InlineData("Hair & Beard Combo", "hair-beard-combo")]
    [InlineData("Hot Towel Shave", "hot-towel-shave")]
    [InlineData("Men's Cut", "mens-cut")]
    [InlineData("  Leading & Trailing  ", "leading-trailing")]
    [InlineData("Multiple   Spaces", "multiple-spaces")]
    [InlineData("UPPERCASE", "uppercase")]
    [InlineData("Café & Spa!", "caf-spa")]
    [InlineData("", "")]
    public void GenerateSlug_ProducesExpectedSlug(string input, string expected)
    {
        var result = SlugHelper.GenerateSlug(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateSlug_WithNull_ReturnsEmpty()
    {
        var result = SlugHelper.GenerateSlug(null!);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateSlug_DoesNotStartOrEndWithHyphen()
    {
        var result = SlugHelper.GenerateSlug("!Test!");
        Assert.False(result.StartsWith('-'));
        Assert.False(result.EndsWith('-'));
    }

    [Fact]
    public void GenerateSlug_DoesNotContainConsecutiveHyphens()
    {
        var result = SlugHelper.GenerateSlug("One & Two & Three");
        Assert.DoesNotContain("--", result);
    }
}
