using BookIt.Payments.Stripe;
using Microsoft.Extensions.Logging;

namespace BookIt.Payments.ApplePay;

/// <summary>
/// Apple Pay provider backed by Stripe Payment Intents.
/// All Apple Pay payments on the web are processed through Stripe — this provider
/// creates a PaymentIntent with the <c>card</c> payment method type (which Stripe
/// automatically extends to Apple Pay via the Payment Request API) and an explicit
/// <c>apple_pay</c> payment method option so Stripe shows the Apple Pay button.
/// </summary>
public sealed class ApplePayProvider : IApplePayProvider
{
    private readonly IStripeProvider _stripeProvider;
    private readonly ILogger<ApplePayProvider> _logger;

    public ApplePayProvider(IStripeProvider stripeProvider, ILogger<ApplePayProvider> logger)
    {
        _stripeProvider = stripeProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ApplePayIntentResult> CreateApplePayIntentAsync(
        string stripeSecretKey,
        long amountInSmallestUnit,
        string currency,
        Dictionary<string, string>? metadata = null)
    {
        var meta = new Dictionary<string, string>(metadata ?? new Dictionary<string, string>())
        {
            ["payment_method"] = "apple_pay"
        };

        var result = await _stripeProvider.CreatePaymentIntentAsync(
            stripeSecretKey,
            amountInSmallestUnit,
            currency,
            meta);

        _logger.LogInformation(
            "Created Apple Pay PaymentIntent {Id} for {Amount} {Currency}",
            result.PaymentIntentId, amountInSmallestUnit, currency);

        return new ApplePayIntentResult(result.PaymentIntentId, result.ClientSecret);
    }

    /// <inheritdoc />
    public async Task RefundAsync(
        string stripeSecretKey,
        string paymentIntentId,
        long? amountInSmallestUnit = null)
    {
        await _stripeProvider.RefundAsync(stripeSecretKey, paymentIntentId, amountInSmallestUnit);

        _logger.LogInformation(
            "Apple Pay refund issued for PaymentIntent {PaymentIntentId}", paymentIntentId);
    }
}
