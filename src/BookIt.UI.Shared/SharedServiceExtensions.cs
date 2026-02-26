using BookIt.UI.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace BookIt.UI.Shared;

public static class SharedServiceExtensions
{
    /// <summary>
    /// Registers all services required by BookIt.UI.Shared.
    /// Call this from any host (Blazor Server, MAUI, etc.).
    /// </summary>
    public static IServiceCollection AddBookItUI(
        this IServiceCollection services,
        string apiBaseUrl)
    {
        services.AddMudServices();

        services.AddHttpClient<BookItApiService>(c =>
            c.BaseAddress = new Uri(apiBaseUrl));

        services.AddScoped<BookItAuthState>();

        return services;
    }
}
