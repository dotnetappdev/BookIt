using BookIt.Maui.Services;
using BookIt.UI.Shared;
using BookIt.UI.Shared.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace BookIt.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
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
