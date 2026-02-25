using BookIt.Core.Enums;

namespace BookIt.Core;

/// <summary>
/// Determines which features are available based on a tenant's subscription plan.
/// </summary>
public static class FeatureFlags
{
    public static bool CanUseOnlinePayments(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Starter;

    public static bool CanUseCustomForms(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Starter;

    public static bool CanUseAiAssistant(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Pro;

    public static bool CanUseMultipleStaff(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Starter;

    public static bool CanUseApiAccess(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Pro;

    public static bool CanUseInterviews(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Pro;

    public static bool CanRemoveBranding(SubscriptionPlan plan) =>
        plan >= SubscriptionPlan.Enterprise;

    public static int MaxServices(SubscriptionPlan plan) => plan switch
    {
        SubscriptionPlan.Free => 3,
        SubscriptionPlan.Starter => 10,
        SubscriptionPlan.Pro => 50,
        SubscriptionPlan.Enterprise => int.MaxValue,
        _ => 3
    };

    public static int MaxStaff(SubscriptionPlan plan) => plan switch
    {
        SubscriptionPlan.Free => 1,
        SubscriptionPlan.Starter => 5,
        SubscriptionPlan.Pro => 25,
        SubscriptionPlan.Enterprise => int.MaxValue,
        _ => 1
    };
}
