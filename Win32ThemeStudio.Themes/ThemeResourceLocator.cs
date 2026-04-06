using System.Windows;

namespace Win32ThemeStudio.Themes;

public static class ThemeResourceLocator
{
    public static Uri BaseResources => CreateAbsoluteUri(ThemeCatalog.BaseStylesUri);

    public static Uri LightColorScheme => CreateAbsoluteUri(ThemeCatalog.DefaultLightThemeUri);

    public static Uri DarkColorScheme => CreateAbsoluteUri(ThemeCatalog.DefaultDarkThemeUri);

    public static void EnsureBaseResources(ResourceDictionary rootResourceDictionary)
    {
        ArgumentNullException.ThrowIfNull(rootResourceDictionary);

        if (FindContainedResourceDictionaryByUri(rootResourceDictionary, BaseResources) is not null)
        {
            return;
        }

        rootResourceDictionary.MergedDictionaries.Insert(0, new ResourceDictionary
        {
            Source = BaseResources
        });
    }

    public static void SetTheme(ResourceDictionary rootResourceDictionary, string themeNameOrId)
    {
        ArgumentNullException.ThrowIfNull(rootResourceDictionary);

        ReplaceThemeResources(rootResourceDictionary, CreateTaggedThemeResource(ThemeCatalog.GetTheme(themeNameOrId)));
    }

    public static void SetTheme(ResourceDictionary rootResourceDictionary, ThemeDescriptor theme)
    {
        ArgumentNullException.ThrowIfNull(rootResourceDictionary);
        ArgumentNullException.ThrowIfNull(theme);

        ReplaceThemeResources(rootResourceDictionary, CreateTaggedThemeResource(theme));
    }

    public static void ReplaceThemeResources(ResourceDictionary rootResourceDictionary, ResourceDictionary themeResourceDictionary)
    {
        ArgumentNullException.ThrowIfNull(rootResourceDictionary);
        ArgumentNullException.ThrowIfNull(themeResourceDictionary);

        var currentTheme = FindFirstContainedPaletteResourceDictionary(rootResourceDictionary);
        rootResourceDictionary.MergedDictionaries.Add(themeResourceDictionary);

        if (currentTheme is not null)
        {
            _ = RemoveResourceDictionaryFromResourcesDeep(currentTheme, rootResourceDictionary);
        }
    }

    private static ResourceDictionary? FindContainedResourceDictionaryByUri(ResourceDictionary resourceDictionary, Uri resourceUri)
    {
        if (resourceDictionary.Source is not null &&
            resourceDictionary.Source.IsAbsoluteUri &&
            string.Equals(resourceDictionary.Source.AbsoluteUri, resourceUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
        {
            return resourceDictionary;
        }

        foreach (var mergedDictionary in resourceDictionary.MergedDictionaries)
        {
            var match = FindContainedResourceDictionaryByUri(mergedDictionary, resourceUri);
            if (match is not null)
            {
                return match;
            }
        }

        return null;
    }

    private static ResourceDictionary? FindFirstContainedPaletteResourceDictionary(ResourceDictionary resourceDictionary)
    {
        if (resourceDictionary.Contains(ThemePaletteKeys.ThemeIsPaletteKey))
        {
            return resourceDictionary;
        }

        foreach (var mergedDictionary in resourceDictionary.MergedDictionaries)
        {
            var palette = FindFirstContainedPaletteResourceDictionary(mergedDictionary);
            if (palette is not null)
            {
                return palette;
            }
        }

        return null;
    }

    private static bool RemoveResourceDictionaryFromResourcesDeep(ResourceDictionary resourceDictionaryToRemove, ResourceDictionary rootResourceDictionary)
    {
        if (rootResourceDictionary.MergedDictionaries.Contains(resourceDictionaryToRemove))
        {
            rootResourceDictionary.MergedDictionaries.Remove(resourceDictionaryToRemove);
            return true;
        }

        foreach (var mergedDictionary in rootResourceDictionary.MergedDictionaries)
        {
            if (RemoveResourceDictionaryFromResourcesDeep(resourceDictionaryToRemove, mergedDictionary))
            {
                return true;
            }
        }

        return false;
    }

    private static Uri CreateAbsoluteUri(string value)
    {
        return new Uri(value, UriKind.Absolute);
    }

    private static ResourceDictionary CreateTaggedThemeResource(ThemeDescriptor theme)
    {
        var resourceDictionary = new ResourceDictionary
        {
            [ThemePaletteKeys.ThemeIsPaletteKey] = "True",
            [ThemePaletteKeys.ThemeDisplayNameKey] = theme.DisplayName,
            [ThemePaletteKeys.ThemeIdKey] = theme.Id
        };

        resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = CreateAbsoluteUri(theme.ResourceUri)
        });

        return resourceDictionary;
    }
}