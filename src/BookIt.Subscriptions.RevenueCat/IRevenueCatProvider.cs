using BookIt.Core.Enums;

namespace BookIt.Subscriptions.RevenueCat;

/// <summary>Result of a RevenueCat customer lookup or creation.</summary>
public sealed record RevenueCatCustomer(
    string CustomerId,
    SubscriptionPlan ActivePlan,
    bool IsActive,
    DateTime? ExpiresAt);

/// <summary>Represents a subscription tier offered through RevenueCat.</summary>
public sealed record RevenueCatTier(
    SubscriptionPlan Plan,
    string ProductId,
    string MonthlyProductId,
    string AnnualProductId,
    decimal MonthlyPrice,
    decimal AnnualPrice,
    string Currency);

/// <summary>
/// Abstraction over the RevenueCat REST API.
/// Accepts credentials per-call so a single instance can serve multiple tenants.
/// </summary>
public interface IRevenueCatProvider
{
    /// <summary>Retrieves or creates a RevenueCat customer by app user ID.</summary>
    Task<RevenueCatCustomer> GetOrCreateCustomerAsync(
        string apiKey,
        string appUserId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the active entitlement plan for the given customer.
    /// Returns <see cref="SubscriptionPlan.Free"/> if no paid entitlement is found.
    /// </summary>
    Task<SubscriptionPlan> GetEntitlementPlanAsync(
        string apiKey,
        string appUserId,
        string entitlementId = "premium",
        CancellationToken cancellationToken = default);

    /// <summary>Returns all available subscription tiers configured on the platform.</summary>
    Task<IReadOnlyList<RevenueCatTier>> GetOfferingsAsync(
        string apiKey,
        CancellationToken cancellationToken = default);
}
