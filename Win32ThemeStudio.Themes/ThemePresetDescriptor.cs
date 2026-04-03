namespace Win32ThemeStudio.Themes;

public sealed class ThemePresetDescriptor
{
    public string DisplayName { get; init; } = string.Empty;

    public ThemeAppearance Appearance { get; init; } = ThemeAppearance.Light;

    public string Category { get; init; } = string.Empty;

    public string AccentFamily { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string[] Tags { get; init; } = [];

    public string? SourceThemeUri { get; init; }

    public static ThemePresetDescriptor FromThemeDescriptor(ThemeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        return new ThemePresetDescriptor
        {
            DisplayName = descriptor.DisplayName,
            Appearance = descriptor.Appearance,
            Category = descriptor.Category,
            AccentFamily = descriptor.AccentFamily,
            Description = descriptor.Description,
            Tags = descriptor.Tags.ToArray(),
            SourceThemeUri = descriptor.ResourceUri
        };
    }

    public ThemeDescriptor ToThemeDescriptor()
    {
        var resourceUri = string.IsNullOrWhiteSpace(SourceThemeUri)
            ? $"theme-preset://custom/{Uri.EscapeDataString(DisplayName)}"
            : SourceThemeUri;

        return new ThemeDescriptor(
            DisplayName,
            resourceUri,
            Appearance,
            Category,
            AccentFamily,
            Description,
            Tags);
    }
}