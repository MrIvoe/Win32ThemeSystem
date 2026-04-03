using System.Collections.ObjectModel;

namespace Win32ThemeStudio.Themes;

public sealed class ThemeDescriptor
{
    public ThemeDescriptor(
        string displayName,
        string resourceUri,
        ThemeAppearance appearance,
        string category,
        string accentFamily,
        string description,
        IEnumerable<string> tags)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(accentFamily);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentNullException.ThrowIfNull(tags);

        DisplayName = displayName;
        ResourceUri = resourceUri;
        Appearance = appearance;
        Category = category;
        AccentFamily = accentFamily;
        Description = description;
        Tags = Array.AsReadOnly(tags
            .Where(static tag => !string.IsNullOrWhiteSpace(tag))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray());
    }

    public string DisplayName { get; }

    public string ResourceUri { get; }

    public ThemeAppearance Appearance { get; }

    public string Category { get; }

    public string AccentFamily { get; }

    public string Description { get; }

    public ReadOnlyCollection<string> Tags { get; }

    public bool HasTag(string tag)
    {
        return Tags.Contains(tag, StringComparer.OrdinalIgnoreCase);
    }

    public override string ToString() => DisplayName;
}