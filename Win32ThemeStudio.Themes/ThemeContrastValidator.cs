using System.Windows.Media;

namespace Win32ThemeStudio.Themes;

public static class ThemeContrastValidator
{
    private static readonly ContrastRule[] Rules =
    [
        new(ThemePaletteKeys.TextPrimary, ThemePaletteKeys.Background, 4.5, "Primary text should remain readable on the window background."),
        new(ThemePaletteKeys.TextPrimary, ThemePaletteKeys.Surface, 4.5, "Primary text should remain readable on main surfaces."),
        new(ThemePaletteKeys.TextPrimary, ThemePaletteKeys.SurfaceAlt, 4.5, "Primary text should remain readable on elevated surfaces."),
        new(ThemePaletteKeys.TextPrimary, ThemePaletteKeys.TooltipBackground, 4.5, "Tooltip text should remain readable."),
        new(ThemePaletteKeys.TextPrimary, ThemePaletteKeys.MenuBackground, 4.5, "Menu text should remain readable."),
        new(ThemePaletteKeys.AccentForeground, ThemePaletteKeys.Accent, 4.5, "Accent buttons should keep labels legible."),
        new(ThemePaletteKeys.WarningForeground, ThemePaletteKeys.Warning, 4.5, "Warning actions should keep labels legible."),
        new(ThemePaletteKeys.DangerForeground, ThemePaletteKeys.Danger, 4.5, "Danger actions should keep labels legible."),
        new(ThemePaletteKeys.SuccessForeground, ThemePaletteKeys.Success, 4.5, "Success actions should keep labels legible."),
        new(ThemePaletteKeys.TextSecondary, ThemePaletteKeys.Background, 3.0, "Secondary text should remain comfortably legible against the background."),
        new(ThemePaletteKeys.TextSecondary, ThemePaletteKeys.Surface, 3.0, "Secondary text should remain comfortably legible on surfaces."),
        new(ThemePaletteKeys.TextDisabled, ThemePaletteKeys.DisabledSurface, 2.0, "Disabled text should stay readable without competing with active controls.")
    ];

    public static IReadOnlyList<ThemeContrastIssue> ValidateTheme(string themeName)
    {
        return ValidatePalette(ThemePaletteSnapshot.FromTheme(themeName));
    }

    public static IReadOnlyList<ThemeContrastIssue> ValidatePreset(ThemePreset preset)
    {
        ArgumentNullException.ThrowIfNull(preset);

        return ValidatePalette(preset.ToPaletteSnapshot());
    }

    public static IReadOnlyList<ThemeContrastIssue> ValidatePalette(ThemePaletteSnapshot palette)
    {
        ArgumentNullException.ThrowIfNull(palette);

        var issues = new List<ThemeContrastIssue>();
        foreach (var rule in Rules)
        {
            var contrastRatio = CalculateContrastRatio(
                palette.GetColor(rule.ForegroundKey),
                palette.GetColor(rule.BackgroundKey));

            if (contrastRatio >= rule.RequiredContrastRatio)
            {
                continue;
            }

            issues.Add(new ThemeContrastIssue(
                rule.ForegroundKey,
                rule.BackgroundKey,
                contrastRatio,
                rule.RequiredContrastRatio,
                GetSeverity(contrastRatio, rule.RequiredContrastRatio),
                rule.Message));
        }

        return issues;
    }

    private static ThemeContrastSeverity GetSeverity(double contrastRatio, double requiredContrastRatio)
    {
        return contrastRatio < requiredContrastRatio * 0.75
            ? ThemeContrastSeverity.Critical
            : ThemeContrastSeverity.Warning;
    }

    private static double CalculateContrastRatio(Color foreground, Color background)
    {
        var foregroundLuminance = GetRelativeLuminance(foreground);
        var backgroundLuminance = GetRelativeLuminance(background);
        var lighter = Math.Max(foregroundLuminance, backgroundLuminance);
        var darker = Math.Min(foregroundLuminance, backgroundLuminance);
        return (lighter + 0.05) / (darker + 0.05);
    }

    private static double GetRelativeLuminance(Color color)
    {
        var red = ToLinear(color.R / 255d);
        var green = ToLinear(color.G / 255d);
        var blue = ToLinear(color.B / 255d);
        return (0.2126 * red) + (0.7152 * green) + (0.0722 * blue);
    }

    private static double ToLinear(double channel)
    {
        return channel <= 0.03928
            ? channel / 12.92
            : Math.Pow((channel + 0.055) / 1.055, 2.4);
    }

    private readonly record struct ContrastRule(
        string ForegroundKey,
        string BackgroundKey,
        double RequiredContrastRatio,
        string Message);
}