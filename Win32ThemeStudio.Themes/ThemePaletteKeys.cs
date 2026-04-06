using System.Collections.ObjectModel;

namespace Win32ThemeStudio.Themes;

public static class ThemePaletteKeys
{
    public const string ThemeIsPaletteKey = "Theme.IsPalette";
    public const string ThemeDisplayNameKey = "Theme.DisplayName";
    public const string ThemeIdKey = "Theme.Id";

    public const string BrushKeyPrefix = "Brush.";
    public const string Background = "Brush.Background";
    public const string Surface = "Brush.Surface";
    public const string SurfaceAlt = "Brush.SurfaceAlt";
    public const string WindowGlass = "Brush.WindowGlass";
    public const string TransparentLayer = "Brush.TransparentLayer";
    public const string TextPrimary = "Brush.TextPrimary";
    public const string TextSecondary = "Brush.TextSecondary";
    public const string TextDisabled = "Brush.TextDisabled";
    public const string Accent = "Brush.Accent";
    public const string AccentSecondary = "Brush.AccentSecondary";
    public const string AccentTertiary = "Brush.AccentTertiary";
    public const string AccentHover = "Brush.AccentHover";
    public const string AccentPressed = "Brush.AccentPressed";
    public const string AccentForeground = "Brush.AccentForeground";
    public const string SelectionFill = "Brush.SelectionFill";
    public const string FocusStroke = "Brush.FocusStroke";
    public const string Border = "Brush.Border";
    public const string ScrollbarThumb = "Brush.ScrollbarThumb";
    public const string TooltipBackground = "Brush.TooltipBackground";
    public const string MenuBackground = "Brush.MenuBackground";
    public const string MenuHover = "Brush.MenuHover";
    public const string DisabledSurface = "Brush.DisabledSurface";
    public const string Danger = "Brush.Danger";
    public const string DangerForeground = "Brush.DangerForeground";
    public const string Warning = "Brush.Warning";
    public const string WarningForeground = "Brush.WarningForeground";
    public const string Success = "Brush.Success";
    public const string SuccessForeground = "Brush.SuccessForeground";
    public const string Shadow = "Brush.Shadow";

    public static ReadOnlyCollection<string> RequiredBrushKeys { get; } =
        Array.AsReadOnly(
        [
            Background,
            Surface,
            SurfaceAlt,
            WindowGlass,
            TransparentLayer,
            TextPrimary,
            TextSecondary,
            TextDisabled,
            Accent,
            AccentSecondary,
            AccentTertiary,
            AccentHover,
            AccentPressed,
            AccentForeground,
            SelectionFill,
            FocusStroke,
            Border,
            ScrollbarThumb,
            TooltipBackground,
            MenuBackground,
            MenuHover,
            DisabledSurface,
            Danger,
            DangerForeground,
            Warning,
            WarningForeground,
            Success,
            SuccessForeground,
            Shadow
        ]);

    public static bool IsBrushKey(string resourceKey)
    {
        return !string.IsNullOrWhiteSpace(resourceKey) &&
               resourceKey.StartsWith(BrushKeyPrefix, StringComparison.Ordinal);
    }
}