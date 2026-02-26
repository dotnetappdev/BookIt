using BookIt.Core.Enums;
using MudBlazor;

namespace BookIt.UI.Shared;

public static class BookItTheme
{
    public static MudTheme Default { get; } = BuildTheme("#6c5ce7", "#a29bfe");

    /// <summary>Returns the MudTheme whose primary palette matches the tenant's chosen theme.</summary>
    public static MudTheme FromTenantTheme(TenantTheme theme) => theme switch
    {
        TenantTheme.Ocean    => BuildTheme("#0ea5e9", "#38bdf8"),
        TenantTheme.Forest   => BuildTheme("#16a34a", "#4ade80"),
        TenantTheme.Sunset   => BuildTheme("#f97316", "#fb923c"),
        TenantTheme.Rose     => BuildTheme("#e11d48", "#fb7185"),
        TenantTheme.Midnight => BuildTheme("#334155", "#64748b"),
        _                    => Default,  // Indigo
    };

    /// <summary>Primary hex colour for a given theme (for use in inline styles).</summary>
    public static string PrimaryOf(TenantTheme theme) => theme switch
    {
        TenantTheme.Ocean    => "#0ea5e9",
        TenantTheme.Forest   => "#16a34a",
        TenantTheme.Sunset   => "#f97316",
        TenantTheme.Rose     => "#e11d48",
        TenantTheme.Midnight => "#334155",
        _                    => "#6c5ce7",
    };

    /// <summary>Secondary hex colour for a given theme (for use in inline styles).</summary>
    public static string SecondaryOf(TenantTheme theme) => theme switch
    {
        TenantTheme.Ocean    => "#38bdf8",
        TenantTheme.Forest   => "#4ade80",
        TenantTheme.Sunset   => "#fb923c",
        TenantTheme.Rose     => "#fb7185",
        TenantTheme.Midnight => "#64748b",
        _                    => "#a29bfe",
    };

    private static MudTheme BuildTheme(string primary, string secondary) => new MudTheme
    {
        PaletteLight = new PaletteLight
        {
            Primary = primary,
            PrimaryDarken = DarkenHex(primary),
            PrimaryLighten = secondary,
            Secondary = secondary,
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
            Primary = LightenHex(primary),
            PrimaryDarken = primary,
            PrimaryLighten = secondary,
            Secondary = secondary,
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

    // Simple darkening / lightening helpers (no external deps)
    private static string DarkenHex(string hex) => ShiftBrightness(hex, -0.15f);
    private static string LightenHex(string hex) => ShiftBrightness(hex, 0.12f);

    private static string ShiftBrightness(string hex, float delta)
    {
        hex = hex.TrimStart('#');
        if (hex.Length != 6) return "#" + hex;
        byte r = Convert.ToByte(hex[..2], 16);
        byte g = Convert.ToByte(hex[2..4], 16);
        byte b = Convert.ToByte(hex[4..6], 16);
        int shift = (int)(255 * delta);
        r = Clamp(r + shift);
        g = Clamp(g + shift);
        b = Clamp(b + shift);
        return $"#{r:x2}{g:x2}{b:x2}";
    }

    private static byte Clamp(int v) => (byte)Math.Clamp(v, 0, 255);
}
