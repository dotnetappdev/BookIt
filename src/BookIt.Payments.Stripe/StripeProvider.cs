using Microsoft.Extensions.Logging;
using StripeLib = global::Stripe;

namespace BookIt.Payments.Stripe;

/// <summary>
/// Stripe-specific payment provider. Wraps stripe.net and keeps all Stripe SDK
/// references isolated from the rest of the application.
/// </summary>
public sealed class StripeProvider : IStripeProvider
{
    private readonly ILogger<StripeProvider> _logger;

    public StripeProvider(ILogger<StripeProvider> logger)
    {
        _logger = logger;
    }

    public async Task<PaymentIntentResult> CreatePaymentIntentAsync(
        string secretKey,
        long amountInSmallestUnit,
        string currency,
        Dictionary<string, string>? metadata = null)
    {
        StripeLib.StripeConfiguration.ApiKey = secretKey;

        var options = new StripeLib.PaymentIntentCreateOptions
        {
            Amount = amountInSmallestUnit,
            Currency = currency.ToLowerInvariant(),
            Metadata = metadata
        };

        var service = new StripeLib.PaymentIntentService();
        var intent = await service.CreateAsync(options);

        _logger.LogInformation("Created Stripe PaymentIntent {Id} for {Amount} {Currency}",
            intent.Id, amountInSmallestUnit, currency);

        return new PaymentIntentResult(intent.Id, intent.ClientSecret ?? intent.Id);
    }

    public async Task RefundAsync(
        string secretKey,
        string paymentIntentId,
        long? amountInSmallestUnit = null)
    {
        StripeLib.StripeConfiguration.ApiKey = secretKey;

        var options = new StripeLib.RefundCreateOptions
        {
            PaymentIntent = paymentIntentId,
            Amount = amountInSmallestUnit
        };

        var service = new StripeLib.RefundService();
        var refund = await service.CreateAsync(options);

        _logger.LogInformation("Stripe refund {Id} issued for PaymentIntent {PaymentIntentId}",
            refund.Id, paymentIntentId);
    }
}
