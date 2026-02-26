using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Notifications.Email;

public static class EmailServiceExtensions
{
    /// <summary>
    /// Registers <see cref="IEmailNotificationService"/> (SendGrid) in the DI container.
    /// </summary>
    public static IServiceCollection AddSendGridEmail(this IServiceCollection services)
    {
        services.AddScoped<IEmailNotificationService, SendGridEmailService>();
        return services;
    }
}
