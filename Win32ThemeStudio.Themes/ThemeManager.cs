using System.Windows;

namespace Win32ThemeStudio.Themes;

public static class ThemeManager
{
    public static IReadOnlyDictionary<string, string> AvailableThemes => ThemeCatalog.ThemeUris;

    public static ResourceDictionary CreateBaseStylesDictionary()
    {
        return new ResourceDictionary
        {
            Source = new Uri(ThemeCatalog.BaseStylesUri, UriKind.Absolute)
        };
    }

    public static ResourceDictionary CreateThemeDictionary(string themeName)
    {
        return new ResourceDictionary
        {
            Source = new Uri(ThemeCatalog.GetThemeUri(themeName), UriKind.Absolute)
        };
    }

    public static ResourceDictionary CreateThemeDictionary(ThemePreset preset)
    {
        ArgumentNullException.ThrowIfNull(preset);

        var resourceDictionary = new ResourceDictionary
        {
            ["Theme.IsPalette"] = "True",
            ["Theme.DisplayName"] = preset.Theme.DisplayName
        };

        foreach (var (resourceKey, colorHex) in preset.ToPaletteSnapshot().BrushValues)
        {
            resourceDictionary[resourceKey] = ThemePaletteSnapshot.CreateBrush(colorHex);
        }

        return resourceDictionary;
    }

    public static void EnsureBaseStyles(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);

        var mergedDictionaries = application.Resources.MergedDictionaries;
        if (mergedDictionaries.Any(static dictionary => UriEquals(dictionary.Source, ThemeCatalog.BaseStylesUri)))
        {
            return;
        }

        mergedDictionaries.Insert(0, CreateBaseStylesDictionary());
    }

    public static void InitializeApplicationTheme(string themeName)
    {
        var application = Application.Current
            ?? throw new InvalidOperationException("Application.Current is unavailable.");

        InitializeApplicationTheme(application, themeName);
    }

    public static void InitializeApplicationTheme(Application application, string themeName)
    {
        ArgumentNullException.ThrowIfNull(application);

        EnsureBaseStyles(application);
        ApplyTheme(application, themeName);
    }

    public static void InitializeApplicationTheme(ThemePreset preset)
    {
        var application = Application.Current
            ?? throw new InvalidOperationException("Application.Current is unavailable.");

        InitializeApplicationTheme(application, preset);
    }

    public static void InitializeApplicationTheme(Application application, ThemePreset preset)
    {
        ArgumentNullException.ThrowIfNull(application);

        EnsureBaseStyles(application);
        ApplyTheme(application, preset);
    }

    public static void ApplyTheme(string themeName)
    {
        var app = Application.Current
            ?? throw new InvalidOperationException("Application.Current is unavailable.");

        ApplyTheme(app, themeName);
    }

    public static void ApplyTheme(Application application, string themeName)
    {
        ArgumentNullException.ThrowIfNull(application);

        var mergedDictionaries = application.Resources.MergedDictionaries;

        for (var i = mergedDictionaries.Count - 1; i >= 0; i--)
        {
            if (mergedDictionaries[i].Contains("Theme.IsPalette"))
            {
                mergedDictionaries.RemoveAt(i);
            }
        }

        mergedDictionaries.Add(CreateThemeDictionary(themeName));
    }

    public static void ApplyTheme(ThemePreset preset)
    {
        var application = Application.Current
            ?? throw new InvalidOperationException("Application.Current is unavailable.");

        ApplyTheme(application, preset);
    }

    public static void ApplyTheme(Application application, ThemePreset preset)
    {
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(preset);

        var mergedDictionaries = application.Resources.MergedDictionaries;

        for (var i = mergedDictionaries.Count - 1; i >= 0; i--)
        {
            if (mergedDictionaries[i].Contains("Theme.IsPalette"))
            {
                mergedDictionaries.RemoveAt(i);
            }
        }

        mergedDictionaries.Add(CreateThemeDictionary(preset));
    }

    private static bool UriEquals(Uri? source, string uri)
    {
        return source is not null &&
               string.Equals(source.AbsoluteUri, uri, StringComparison.OrdinalIgnoreCase);
    }
}
