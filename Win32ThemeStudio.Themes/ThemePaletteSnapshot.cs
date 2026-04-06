using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Win32ThemeStudio.Themes;

public sealed class ThemePaletteSnapshot
{
    public static ReadOnlyCollection<string> RequiredBrushKeys { get; } = ThemePaletteKeys.RequiredBrushKeys;

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
            CollectBrush(entry, brushValues);
        }

        foreach (var mergedDictionary in resourceDictionary.MergedDictionaries)
        {
            CollectBrushes(mergedDictionary, brushValues);
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

    private static void CollectBrushes(ResourceDictionary resourceDictionary, Dictionary<string, string> brushValues)
    {
        foreach (DictionaryEntry entry in resourceDictionary)
        {
            CollectBrush(entry, brushValues);
        }

        foreach (var mergedDictionary in resourceDictionary.MergedDictionaries)
        {
            CollectBrushes(mergedDictionary, brushValues);
        }
    }

    private static void CollectBrush(DictionaryEntry entry, Dictionary<string, string> brushValues)
    {
        if (entry.Key is not string resourceKey || !ThemePaletteKeys.IsBrushKey(resourceKey))
        {
            return;
        }

        if (entry.Value is not SolidColorBrush brush)
        {
            return;
        }

        brushValues[resourceKey] = brush.Color.ToString();
    }
}