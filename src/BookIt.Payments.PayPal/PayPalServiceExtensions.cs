using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Payments.PayPal;

public static class PayPalServiceExtensions
{
    public static IServiceCollection AddPayPalPayments(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IPayPalProvider, PayPalProvider>();
        return services;
    }
}
