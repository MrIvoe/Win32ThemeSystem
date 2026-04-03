using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Win32ThemeStudio.Themes;

public sealed class ThemePaletteSnapshot
{
    public static ReadOnlyCollection<string> RequiredBrushKeys { get; } =
        Array.AsReadOnly(
        [
            "Brush.Background",
            "Brush.Surface",
            "Brush.SurfaceAlt",
            "Brush.WindowGlass",
            "Brush.TransparentLayer",
            "Brush.TextPrimary",
            "Brush.TextSecondary",
            "Brush.Accent",
            "Brush.AccentHover",
            "Brush.AccentPressed",
            "Brush.Border",
            "Brush.TooltipBackground",
            "Brush.MenuBackground",
            "Brush.MenuHover",
            "Brush.Danger",
            "Brush.Success",
            "Brush.Shadow"
        ]);

    public ThemePaletteSnapshot(IEnumerable<KeyValuePair<string, string>> paletteValues)
    {
        ArgumentNullException.ThrowIfNull(paletteValues);

        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in paletteValues)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            values[key] = NormalizeColor(value);
        }

        var missingKeys = RequiredBrushKeys.Where(requiredKey => !values.ContainsKey(requiredKey)).ToArray();
        if (missingKeys.Length > 0)
        {
            throw new InvalidOperationException($"Theme palette is missing required brush keys: {string.Join(", ", missingKeys)}");
        }

        BrushValues = new ReadOnlyDictionary<string, string>(values);
    }

    public ReadOnlyDictionary<string, string> BrushValues { get; }

    public string GetColorHex(string resourceKey)
    {
        if (!BrushValues.TryGetValue(resourceKey, out var colorHex))
        {
            throw new KeyNotFoundException($"Palette key '{resourceKey}' does not exist.");
        }

        return colorHex;
    }

    public Color GetColor(string resourceKey)
    {
        return ParseColor(GetColorHex(resourceKey));
    }

    public static ThemePaletteSnapshot FromTheme(string themeName)
    {
        return FromResourceDictionary(ThemeManager.CreateThemeDictionary(themeName));
    }

    internal static ThemePaletteSnapshot FromResourceDictionary(ResourceDictionary resourceDictionary)
    {
        ArgumentNullException.ThrowIfNull(resourceDictionary);

        var brushValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (DictionaryEntry entry in resourceDictionary)
        {
            if (entry.Key is not string resourceKey || !resourceKey.StartsWith("Brush.", StringComparison.Ordinal))
            {
                continue;
            }

            if (entry.Value is not SolidColorBrush brush)
            {
                continue;
            }

            brushValues[resourceKey] = brush.Color.ToString();
        }

        return new ThemePaletteSnapshot(brushValues);
    }

    internal static SolidColorBrush CreateBrush(string colorHex)
    {
        var brush = new SolidColorBrush(ParseColor(colorHex));
        brush.Freeze();
        return brush;
    }

    private static string NormalizeColor(string colorHex)
    {
        return ParseColor(colorHex).ToString();
    }

    private static Color ParseColor(string colorHex)
    {
        if (string.IsNullOrWhiteSpace(colorHex))
        {
            throw new InvalidOperationException("Palette color values cannot be empty.");
        }

        if (ColorConverter.ConvertFromString(colorHex) is not Color color)
        {
            throw new InvalidOperationException($"'{colorHex}' is not a valid color value.");
        }

        return color;
    }
}