using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Payments.Stripe;

public static class StripeServiceExtensions
{
    public static IServiceCollection AddStripePayments(this IServiceCollection services)
    {
        services.AddScoped<IStripeProvider, StripeProvider>();
        return services;
    }
}
