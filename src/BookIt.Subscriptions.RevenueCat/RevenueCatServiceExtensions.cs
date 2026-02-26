using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Subscriptions.RevenueCat;

public static class RevenueCatServiceExtensions
{
    /// <summary>
    /// Registers <see cref="IRevenueCatProvider"/> in the DI container.
    /// </summary>
    public static IServiceCollection AddRevenueCat(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IRevenueCatProvider, RevenueCatProvider>();
        return services;
    }
}
