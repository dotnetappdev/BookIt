using BookIt.Payments.Stripe;

namespace BookIt.Payments.ApplePay;

/// <summary>Result of an Apple Pay payment intent creation.</summary>
public sealed record ApplePayIntentResult(string PaymentIntentId, string ClientSecret);

/// <summary>
/// Abstraction over Apple Pay payments processed through Stripe.
/// Apple Pay on the web uses Stripe Payment Request Button or Stripe.js with
/// <c>payment_method_types: ['card']</c> combined with the Payment Request API —
/// this provider creates the necessary PaymentIntent on the server side.
/// </summary>
public interface IApplePayProvider
{
    /// <summary>
    /// Creates a Stripe PaymentIntent configured to accept Apple Pay.
    /// The returned <see cref="ApplePayIntentResult.ClientSecret"/> is passed to
    /// the Stripe.js Payment Request Button on the client.
    /// </summary>
    Task<ApplePayIntentResult> CreateApplePayIntentAsync(
        string stripeSecretKey,
        long amountInSmallestUnit,
        string currency,
        Dictionary<string, string>? metadata = null);

    /// <summary>Issues a full or partial refund for an Apple Pay payment.</summary>
    Task RefundAsync(
        string stripeSecretKey,
        string paymentIntentId,
        long? amountInSmallestUnit = null);
}
