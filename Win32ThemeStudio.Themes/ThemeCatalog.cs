using System.Collections.ObjectModel;

namespace Win32ThemeStudio.Themes;

public static class ThemeCatalog
{
    public const string BaseStylesUri = "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/BaseStyles.xaml";
    public const string DefaultLightThemeId = "light";
    public const string DefaultDarkThemeId = "dark";
    public const string DefaultLightThemeUri = "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/Light.xaml";
    public const string DefaultDarkThemeUri = "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/Dark.xaml";

    public static ReadOnlyCollection<ThemeDescriptor> Themes { get; } =
        Array.AsReadOnly(
        [
            new ThemeDescriptor(
                "amber-terminal",
                "Amber Terminal",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/AmberTerminal.xaml",
                ThemeAppearance.Dark,
                ThemeCategories.Terminal,
                ThemeAccentFamilies.Amber,
                "Amber phosphor-inspired terminal palette for diagnostics, consoles, and operator tooling.",
                ["amber", "crt", "monochrome", "console"]),
            new ThemeDescriptor(
                "arctic-glass",
                "Arctic Glass",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/ArcticGlass.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Glass,
                ThemeAccentFamilies.Blue,
                "Crisp frosted palette with cool blues for clean desktop utility surfaces.",
                ["glass", "cool", "clean", "frosted"]),
            new ThemeDescriptor(
                DefaultLightThemeId,
                "Aurora Light",
                DefaultLightThemeUri,
                ThemeAppearance.Light,
                ThemeCategories.Glass,
                ThemeAccentFamilies.Blue,
                "Bright default palette with approachable blue accents for general-purpose apps.",
                ["default", "blue", "clean", "general"]),
            new ThemeDescriptor(
                DefaultDarkThemeId,
                "Nocturne Dark",
                DefaultDarkThemeUri,
                ThemeAppearance.Dark,
                ThemeCategories.Office,
                ThemeAccentFamilies.Blue,
                "Balanced dark workspace theme tuned for long-running desktop sessions.",
                ["dark", "workspace", "professional", "balanced"]),
            new ThemeDescriptor(
                "neon-cyberpunk",
                "Neon Cyberpunk",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/NeonCyberpunk.xaml",
                ThemeAppearance.Dark,
                ThemeCategories.Futuristic,
                ThemeAccentFamilies.Neon,
                "High-energy neon contrast palette for dashboards and experimental interfaces.",
                ["neon", "cyberpunk", "dashboard", "high-contrast"]),
            new ThemeDescriptor(
                "brass-steampunk",
                "Brass Steampunk",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/BrassSteampunk.xaml",
                ThemeAppearance.Dark,
                ThemeCategories.Industrial,
                ThemeAccentFamilies.Amber,
                "Warm brass-and-ink palette for antique instrumentation and workshop styling.",
                ["brass", "steampunk", "vintage", "ornate"]),
            new ThemeDescriptor(
                "copper-foundry",
                "Copper Foundry",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/CopperFoundry.xaml",
                ThemeAppearance.Dark,
                ThemeCategories.Industrial,
                ThemeAccentFamilies.Copper,
                "Foundry-inspired copper palette with earthy neutrals for utility apps.",
                ["copper", "workshop", "industrial", "warm"]),
            new ThemeDescriptor(
                "emerald-ledger",
                "Emerald Ledger",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/EmeraldLedger.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Office,
                ThemeAccentFamilies.Emerald,
                "Ledger-style green palette suited to finance, planning, and operational tools.",
                ["emerald", "finance", "operations", "productive"]),
            new ThemeDescriptor(
                "graphite-office",
                "Graphite Office",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/GraphiteOffice.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Office,
                ThemeAccentFamilies.Graphite,
                "Graphite productivity theme for admin panels, control centers, and line-of-business software.",
                ["graphite", "admin", "productivity", "professional"]),
            new ThemeDescriptor(
                "harbor-blue",
                "Harbor Blue",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/HarborBlue.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Office,
                ThemeAccentFamilies.Blue,
                "Structured blue-grey office palette designed for data-heavy windows.",
                ["blue", "data", "office", "structured"]),
            new ThemeDescriptor(
                "ivory-bureau",
                "Ivory Bureau",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/IvoryBureau.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Office,
                ThemeAccentFamilies.Neutral,
                "Paper-and-ink bureau palette for document workflows and enterprise forms.",
                ["paper", "bureau", "documents", "neutral"]),
            new ThemeDescriptor(
                "olive-terminal",
                "Olive Terminal",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/OliveTerminal.xaml",
                ThemeAppearance.Dark,
                ThemeCategories.Terminal,
                ThemeAccentFamilies.Olive,
                "Military-green terminal theme for diagnostics, telemetry, and service consoles.",
                ["olive", "terminal", "telemetry", "console"]),
            new ThemeDescriptor(
                "tape-lo-fi",
                "Tape Lo-Fi",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/TapeLofi.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Retro,
                ThemeAccentFamilies.Neutral,
                "Soft analog-inspired palette with muted tape-era colors and relaxed contrast.",
                ["lofi", "analog", "muted", "nostalgic"]),
            new ThemeDescriptor(
                "pop-colorburst",
                "Pop Colorburst",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/PopColorburst.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Vibrant,
                ThemeAccentFamilies.Pink,
                "Bright accent-driven palette for playful creative tools and demos.",
                ["playful", "bold", "bright", "creative"]),
            new ThemeDescriptor(
                "rose-paper",
                "Rose Paper",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/RosePaper.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Retro,
                ThemeAccentFamilies.Pink,
                "Printed-paper palette with rose accents for editorial and journaling style apps.",
                ["rose", "editorial", "paper", "soft"]),
            new ThemeDescriptor(
                "nova-futuristic",
                "Nova Futuristic",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/NovaFuturistic.xaml",
                ThemeAppearance.Dark,
                ThemeCategories.Futuristic,
                ThemeAccentFamilies.Blue,
                "Sharp sci-fi palette for monitoring interfaces and high-tech control rooms.",
                ["sci-fi", "future", "monitoring", "sharp"]),
            new ThemeDescriptor(
                "forest-organic",
                "Forest Organic",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/ForestOrganic.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Nature,
                ThemeAccentFamilies.Emerald,
                "Natural green-and-earth palette for calm business tools and sustainable brands.",
                ["forest", "organic", "nature", "earth"]),
            new ThemeDescriptor(
                "mono-minimal",
                "Mono Minimal",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/MonoMinimal.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Minimal,
                ThemeAccentFamilies.Neutral,
                "Minimal monochrome palette for distraction-free tooling and compact UI density.",
                ["mono", "minimal", "clean", "focused"]),
            new ThemeDescriptor(
                "storm-steel",
                "Storm Steel",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/StormSteel.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Industrial,
                ThemeAccentFamilies.Steel,
                "Steel-blue palette for infrastructure tools and control panel workflows.",
                ["steel", "infrastructure", "control", "cool"]),
            new ThemeDescriptor(
                "sunset-retro",
                "Sunset Retro",
                "pack://application:,,,/Win32ThemeStudio.Themes;component/Themes/Palettes/SunsetRetro.xaml",
                ThemeAppearance.Light,
                ThemeCategories.Retro,
                ThemeAccentFamilies.Sunset,
                "Sunset arcade palette blending warm retro hues with approachable modern contrast.",
                ["sunset", "arcade", "retro", "warm"])
        ]);

    public static ReadOnlyDictionary<string, ThemeDescriptor> ThemeMetadata { get; } =
        new(Themes.ToDictionary(static theme => theme.DisplayName, StringComparer.OrdinalIgnoreCase));

    public static ReadOnlyDictionary<string, ThemeDescriptor> ThemeMetadataById { get; } =
        new(Themes.ToDictionary(static theme => theme.Id, StringComparer.OrdinalIgnoreCase));

    public static ReadOnlyDictionary<string, string> ThemeUris { get; } =
        new(Themes.ToDictionary(static theme => theme.DisplayName, static theme => theme.ResourceUri, StringComparer.OrdinalIgnoreCase));

    public static ReadOnlyDictionary<string, string> ThemeUrisById { get; } =
        new(Themes.ToDictionary(static theme => theme.Id, static theme => theme.ResourceUri, StringComparer.OrdinalIgnoreCase));

    public static ThemeDescriptor DefaultLightTheme => GetTheme(DefaultLightThemeId);

    public static ThemeDescriptor DefaultDarkTheme => GetTheme(DefaultDarkThemeId);

    public static IReadOnlyList<ThemeDescriptor> GetThemesByAppearance(ThemeAppearance appearance)
    {
        return Themes.Where(theme => theme.Appearance == appearance).ToArray();
    }

    public static IReadOnlyList<ThemeDescriptor> GetThemesByCategory(string category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category);

        return Themes
            .Where(theme => string.Equals(theme.Category, category, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public static IReadOnlyList<ThemeDescriptor> GetThemesByAccentFamily(string accentFamily)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accentFamily);

        return Themes
            .Where(theme => string.Equals(theme.AccentFamily, accentFamily, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public static string GetThemeUri(string themeNameOrId)
    {
        return GetTheme(themeNameOrId).ResourceUri;
    }

    public static ThemeDescriptor GetTheme(string themeNameOrId)
    {
        if (TryGetTheme(themeNameOrId, out var theme))
        {
            return theme;
        }

        throw new ArgumentException($"Theme '{themeNameOrId}' does not exist.", nameof(themeNameOrId));
    }

    public static bool TryGetTheme(string themeNameOrId, out ThemeDescriptor theme)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(themeNameOrId);

        if (ThemeMetadataById.TryGetValue(themeNameOrId, out theme!))
        {
            return true;
        }

        return ThemeMetadata.TryGetValue(themeNameOrId, out theme!);
    }

    public static ThemeDescriptor GetDefaultTheme(ThemeAppearance appearance)
    {
        return appearance == ThemeAppearance.Dark ? DefaultDarkTheme : DefaultLightTheme;
    }
}
