using BookIt.Maui.Services;
using BookIt.UI.Shared;
using BookIt.UI.Shared.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Sentry;

namespace BookIt.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseSentry(o =>
            {
                // Set the SENTRY_DSN environment variable (or update this value) to activate Sentry monitoring.
                o.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN") ?? "";
                o.TracesSampleRate = 0.1;
                o.SendDefaultPii = false;
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Blazor WebView
        builder.Services.AddMauiBlazorWebView();

        // MudBlazor with dark theme
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
        });

        // BookIt shared UI (includes BookItApiService, BookItAuthState)
        builder.Services.AddBookItUI("https://api.bookit.app");

        // MAUI-specific services
        builder.Services.AddSingleton<MauiSyncService>();
        builder.Services.AddSingleton<QrCodeService>();
        builder.Services.AddSingleton<WalletPassService>();
        builder.Services.AddScoped<MauiTokenService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
