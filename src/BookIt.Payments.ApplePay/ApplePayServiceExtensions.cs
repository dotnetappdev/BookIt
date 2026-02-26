using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Payments.ApplePay;

public static class ApplePayServiceExtensions
{
    /// <summary>
    /// Registers <see cref="IApplePayProvider"/> in the DI container.
    /// Requires <c>AddStripePayments()</c> to be called first (or included in
    /// the same composition root) since <see cref="ApplePayProvider"/> delegates
    /// to <see cref="BookIt.Payments.Stripe.IStripeProvider"/>.
    /// </summary>
    public static IServiceCollection AddApplePayPayments(this IServiceCollection services)
    {
        services.AddScoped<IApplePayProvider, ApplePayProvider>();
        return services;
    }
}
