namespace BookIt.Payments.Stripe;

/// <summary>Result of a Stripe PaymentIntent creation.</summary>
public sealed record PaymentIntentResult(string PaymentIntentId, string ClientSecret);

/// <summary>
/// Abstraction over the Stripe API. Accepts credentials per-call so a single
/// instance can serve multiple tenants without storing secrets.
/// </summary>
public interface IStripeProvider
{
    /// <summary>Creates a Stripe PaymentIntent and returns the intent ID and client secret.</summary>
    Task<PaymentIntentResult> CreatePaymentIntentAsync(
        string secretKey,
        long amountInSmallestUnit,
        string currency,
        Dictionary<string, string>? metadata = null);

    /// <summary>Issues a full or partial refund against a PaymentIntent.</summary>
    Task RefundAsync(
        string secretKey,
        string paymentIntentId,
        long? amountInSmallestUnit = null);
}
