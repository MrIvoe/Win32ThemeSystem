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
        : this(
            ThemeIds.CreateStableId(displayName),
            displayName,
            resourceUri,
            appearance,
            category,
            accentFamily,
            description,
            tags)
    {
    }

    public ThemeDescriptor(
        string id,
        string displayName,
        string resourceUri,
        ThemeAppearance appearance,
        string category,
        string accentFamily,
        string description,
        IEnumerable<string> tags)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(accentFamily);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentNullException.ThrowIfNull(tags);

        Id = ThemeIds.CreateStableId(id);
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

    public string Id { get; }

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