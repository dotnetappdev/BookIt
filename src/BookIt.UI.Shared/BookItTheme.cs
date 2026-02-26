using MudBlazor;

namespace BookIt.UI.Shared;

public static class BookItTheme
{
    public static MudTheme Default { get; } = new MudTheme
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#6c5ce7",
            PrimaryDarken = "#5549d1",
            PrimaryLighten = "#a29bfe",
            Secondary = "#a29bfe",
            Success = "#10b981",
            Warning = "#f59e0b",
            Error = "#ef4444",
            Info = "#3b82f6",
            Background = "#f8f9fc",
            Surface = "#ffffff",
            AppbarBackground = "rgba(255,255,255,0.97)",
            AppbarText = "#1e293b",
            DrawerBackground = "#16213e",
            DrawerText = "#94a3b8",
            DrawerIcon = "#94a3b8",
            TextPrimary = "#1e293b",
            TextSecondary = "#475569",
            ActionDefault = "#94a3b8",
            TableLines = "#e2e8f0",
            Divider = "#e2e8f0",
            OverlayLight = "rgba(255,255,255,0.5)",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#7c6ff0",
            PrimaryDarken = "#6c5ce7",
            PrimaryLighten = "#a29bfe",
            Secondary = "#a29bfe",
            Success = "#10b981",
            Warning = "#f59e0b",
            Error = "#ef4444",
            Info = "#3b82f6",
            Background = "#0d1117",
            Surface = "#161b22",
            AppbarBackground = "rgba(22,27,34,0.97)",
            AppbarText = "#e6edf3",
            DrawerBackground = "#0d1117",
            DrawerText = "#8b949e",
            DrawerIcon = "#8b949e",
            TextPrimary = "#e6edf3",
            TextSecondary = "#8b949e",
            ActionDefault = "#6e7681",
            TableLines = "#30363d",
            Divider = "#30363d",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = new[] { "Inter", "system-ui", "-apple-system", "sans-serif" },
                FontSize = ".9375rem",
                LineHeight = "1.6",
            },
            H1 = new H1Typography { FontWeight = "800", LetterSpacing = "-.05em" },
            H2 = new H2Typography { FontWeight = "800", LetterSpacing = "-.04em" },
            H3 = new H3Typography { FontWeight = "700", LetterSpacing = "-.03em" },
            H4 = new H4Typography { FontWeight = "700", LetterSpacing = "-.02em" },
            H5 = new H5Typography { FontWeight = "600", LetterSpacing = "-.01em" },
            H6 = new H6Typography { FontWeight = "600" },
        },
        LayoutProperties = new LayoutProperties { DefaultBorderRadius = "12px" },
    };
}
