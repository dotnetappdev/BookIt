using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Notifications.Sms;

public static class SmsServiceExtensions
{
    /// <summary>
    /// Registers both SMS providers in the DI container.
    /// Use <see cref="ClickSendSmsProvider"/> or <see cref="TwilioSmsProvider"/> directly,
    /// or inject <see cref="SmsProviderFactory"/> to select the provider at runtime.
    /// </summary>
    public static IServiceCollection AddSmsNotifications(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<ClickSendSmsProvider>();
        services.AddScoped<TwilioSmsProvider>();
        services.AddScoped<SmsProviderFactory>();
        return services;
    }
}
