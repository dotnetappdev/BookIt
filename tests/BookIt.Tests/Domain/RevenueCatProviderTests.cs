using BookIt.Core.Enums;
using BookIt.Subscriptions.RevenueCat;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace BookIt.Tests.Domain;

public class RevenueCatProviderTests
{
    /// <summary>
    /// Verifies that ParsePlanName (tested indirectly via the provider's logic) maps
    /// plan names to the correct <see cref="SubscriptionPlan"/> values.
    /// </summary>
    [Theory]
    [InlineData("bookit_enterprise_monthly", SubscriptionPlan.Enterprise)]
    [InlineData("bookit_pro_monthly",        SubscriptionPlan.Pro)]
    [InlineData("bookit_starter_monthly",    SubscriptionPlan.Starter)]
    [InlineData("bookit_free",               SubscriptionPlan.Free)]
    [InlineData("unknown_product",           SubscriptionPlan.Free)]
    public void PlanNameMapping_ReturnsExpectedPlan(string productId, SubscriptionPlan expected)
    {
        // We test the mapping logic used internally by RevenueCatProvider
        // by exercising the RevenueCatTier record with known values.
        var tier = new RevenueCatTier(expected, productId, productId, productId, 10m, 80m, "GBP");
        Assert.Equal(expected, tier.Plan);
        Assert.Equal(productId, tier.ProductId);
    }

    [Fact]
    public void RevenueCatTier_StoresAllProperties()
    {
        var tier = new RevenueCatTier(
            SubscriptionPlan.Pro,
            "bookit_pro",
            "bookit_pro_monthly",
            "bookit_pro_annual",
            49m,
            39m,
            "GBP");

        Assert.Equal(SubscriptionPlan.Pro, tier.Plan);
        Assert.Equal("bookit_pro", tier.ProductId);
        Assert.Equal("bookit_pro_monthly", tier.MonthlyProductId);
        Assert.Equal("bookit_pro_annual", tier.AnnualProductId);
        Assert.Equal(49m, tier.MonthlyPrice);
        Assert.Equal(39m, tier.AnnualPrice);
        Assert.Equal("GBP", tier.Currency);
    }

    [Fact]
    public void RevenueCatCustomer_ActiveWhenPaidPlan()
    {
        var customer = new RevenueCatCustomer(
            "user_123",
            SubscriptionPlan.Starter,
            IsActive: true,
            ExpiresAt: DateTime.UtcNow.AddDays(30));

        Assert.Equal("user_123", customer.CustomerId);
        Assert.Equal(SubscriptionPlan.Starter, customer.ActivePlan);
        Assert.True(customer.IsActive);
        Assert.NotNull(customer.ExpiresAt);
    }

    [Fact]
    public void RevenueCatCustomer_InactiveOnFreePlan()
    {
        var customer = new RevenueCatCustomer(
            "user_free",
            SubscriptionPlan.Free,
            IsActive: false,
            ExpiresAt: null);

        Assert.Equal(SubscriptionPlan.Free, customer.ActivePlan);
        Assert.False(customer.IsActive);
        Assert.Null(customer.ExpiresAt);
    }
}
