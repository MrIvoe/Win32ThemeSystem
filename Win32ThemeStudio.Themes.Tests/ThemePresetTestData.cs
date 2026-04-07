using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.Themes.Tests;

internal static class ThemePresetTestData
{
    public static ThemePreset CreateValidPreset(bool includeBackground = true)
    {
        return new ThemePreset
        {
            FormatVersion = ThemePreset.CurrentFormatVersion,
            Theme = new ThemePresetDescriptor
            {
                Id = "signal-night",
                DisplayName = "Signal Night",
                Appearance = ThemeAppearance.Dark,
                Category = "Office",
                AccentFamily = "Amber",
                Description = "Dark operations preset with amber accents.",
                Tags = ["custom", "signal"],
                SourceThemeUri = "theme-preset://custom/signal-night"
            },
            Background = includeBackground
                ? new ThemeBackgroundPreset
                {
                    Mode = "gradient",
                    PrimaryColor = "#FF13171C",
                    SecondaryColor = "#FF1D232C",
                    SizingMode = "fill",
                    TintColor = "#40171C23",
                    Opacity = 0.92,
                    BlurEnabled = true
                }
                : null,
            PaletteValues = CreateCompletePalette()
        };
    }

    public static Dictionary<string, string> CreateCompletePalette()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Brush.Background"] = "#FF13171C",
            ["Brush.Surface"] = "#FF1D232C",
            ["Brush.SurfaceAlt"] = "#FF252C36",
            ["Brush.WindowGlass"] = "#CC171C23",
            ["Brush.TransparentLayer"] = "#66252C36",
            ["Brush.TextPrimary"] = "#FFE8EDF4",
            ["Brush.TextSecondary"] = "#FF9CA9B8",
            ["Brush.TextDisabled"] = "#FF6D7986",
            ["Brush.Accent"] = "#FFF5AE42",
            ["Brush.AccentSecondary"] = "#FFFFC968",
            ["Brush.AccentTertiary"] = "#FF826C41",
            ["Brush.AccentHover"] = "#FFF8BF63",
            ["Brush.AccentPressed"] = "#FFD89524",
            ["Brush.AccentForeground"] = "#FF251907",
            ["Brush.SelectionFill"] = "#40F5AE42",
            ["Brush.FocusStroke"] = "#FFFFC968",
            ["Brush.Border"] = "#FF323A45",
            ["Brush.ScrollbarThumb"] = "#FF4D5866",
            ["Brush.TooltipBackground"] = "#FF1C2028",
            ["Brush.MenuBackground"] = "#FF181C22",
            ["Brush.MenuHover"] = "#FF252C36",
            ["Brush.DisabledSurface"] = "#FF1C2026",
            ["Brush.Danger"] = "#FFC97A74",
            ["Brush.DangerForeground"] = "#FF260F0D",
            ["Brush.Warning"] = "#FFD1A255",
            ["Brush.WarningForeground"] = "#FF31220A",
            ["Brush.Success"] = "#FF6CA288",
            ["Brush.SuccessForeground"] = "#FF102119",
            ["Brush.Shadow"] = "#48070A10"
        };
    }
}