using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Win32ThemeStudio.Themes;

public static partial class ThemePresetValidator
{
    private static readonly HashSet<string> SupportedBackgroundModes = new(StringComparer.OrdinalIgnoreCase)
    {
        "solid",
        "gradient",
        "image"
    };

    private static readonly HashSet<string> SupportedSizingModes = new(StringComparer.OrdinalIgnoreCase)
    {
        "fill",
        "fit",
        "stretch",
        "tile",
        "center"
    };

    public static IReadOnlyList<ThemePresetValidationIssue> Validate(ThemePreset preset)
    {
        ArgumentNullException.ThrowIfNull(preset);

        var issues = new List<ThemePresetValidationIssue>();
        ValidateFormatVersion(preset.FormatVersion, issues);
        ValidateThemeMetadata(preset.Theme, issues);
        ValidatePaletteValues(preset.PaletteValues, issues);

        if (preset.Background is not null)
        {
            ValidateBackground(preset.Background.Normalize(), issues);
        }

        return issues;
    }

    public static void EnsureValid(ThemePreset preset)
    {
        var issues = Validate(preset);
        var blockingIssues = issues
            .Where(static issue => issue.Severity == ThemePresetValidationSeverity.Error)
            .ToArray();

        if (blockingIssues.Length == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            "Preset validation failed:\n- " +
            string.Join("\n- ", blockingIssues.Select(static issue => $"{issue.Path}: {issue.Message}")));
    }

    private static void ValidateFormatVersion(string? formatVersion, List<ThemePresetValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(formatVersion))
        {
            issues.Add(new ThemePresetValidationIssue(
                "formatVersion",
                ThemePresetValidationSeverity.Error,
                "Format version is required."));
            return;
        }

        if (!string.Equals(formatVersion, ThemePreset.CurrentFormatVersion, StringComparison.Ordinal))
        {
            issues.Add(new ThemePresetValidationIssue(
                "formatVersion",
                ThemePresetValidationSeverity.Error,
                $"Unsupported format version '{formatVersion}'. Expected '{ThemePreset.CurrentFormatVersion}'."));
        }
    }

    private static void ValidateThemeMetadata(ThemePresetDescriptor descriptor, List<ThemePresetValidationIssue> issues)
    {
        ValidateRequiredText(descriptor.Id, "theme.id", issues);
        if (!string.IsNullOrWhiteSpace(descriptor.Id) && !ThemeIdPattern().IsMatch(descriptor.Id))
        {
            issues.Add(new ThemePresetValidationIssue(
                "theme.id",
                ThemePresetValidationSeverity.Error,
                "Theme id must use lowercase stable-id format like 'signal-night'."));
        }

        ValidateRequiredText(descriptor.DisplayName, "theme.displayName", issues);
        ValidateRequiredText(descriptor.Category, "theme.category", issues);
        ValidateRequiredText(descriptor.AccentFamily, "theme.accentFamily", issues);
        ValidateRequiredText(descriptor.Description, "theme.description", issues);

        if (descriptor.Tags.Any(static tag => string.IsNullOrWhiteSpace(tag)))
        {
            issues.Add(new ThemePresetValidationIssue(
                "theme.tags",
                ThemePresetValidationSeverity.Error,
                "Theme tags cannot contain blank values."));
        }

        var distinctTagCount = descriptor.Tags
            .Where(static tag => !string.IsNullOrWhiteSpace(tag))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
        if (distinctTagCount != descriptor.Tags.Count(static tag => !string.IsNullOrWhiteSpace(tag)))
        {
            issues.Add(new ThemePresetValidationIssue(
                "theme.tags",
                ThemePresetValidationSeverity.Error,
                "Theme tags must be unique."));
        }
    }

    private static void ValidatePaletteValues(
        Dictionary<string, string>? paletteValues,
        List<ThemePresetValidationIssue> issues)
    {
        if (paletteValues is null || paletteValues.Count == 0)
        {
            issues.Add(new ThemePresetValidationIssue(
                "paletteValues",
                ThemePresetValidationSeverity.Error,
                "Palette values are required."));
            return;
        }

        foreach (var requiredKey in ThemePaletteKeys.RequiredBrushKeys)
        {
            if (!paletteValues.ContainsKey(requiredKey))
            {
                issues.Add(new ThemePresetValidationIssue(
                    $"paletteValues.{requiredKey}",
                    ThemePresetValidationSeverity.Error,
                    "Required brush token is missing."));
            }
        }

        foreach (var (key, value) in paletteValues)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                issues.Add(new ThemePresetValidationIssue(
                    "paletteValues",
                    ThemePresetValidationSeverity.Error,
                    "Palette token names cannot be blank."));
                continue;
            }

            if (!IsHexColor(value))
            {
                issues.Add(new ThemePresetValidationIssue(
                    $"paletteValues.{key}",
                    ThemePresetValidationSeverity.Error,
                    "Palette values must be hex colors in #RRGGBB or #AARRGGBB format."));
            }
        }
    }

    private static void ValidateBackground(ThemeBackgroundPreset background, List<ThemePresetValidationIssue> issues)
    {
        if (!SupportedBackgroundModes.Contains(background.Mode))
        {
            issues.Add(new ThemePresetValidationIssue(
                "background.mode",
                ThemePresetValidationSeverity.Error,
                "Background mode must be solid, gradient, or image."));
        }

        if (!SupportedSizingModes.Contains(background.SizingMode))
        {
            issues.Add(new ThemePresetValidationIssue(
                "background.sizingMode",
                ThemePresetValidationSeverity.Error,
                "Background sizing mode must be fill, fit, stretch, tile, or center."));
        }

        ValidateOptionalColor(background.PrimaryColor, "background.primaryColor", issues);
        ValidateOptionalColor(background.SecondaryColor, "background.secondaryColor", issues);
        ValidateOptionalColor(background.TintColor, "background.tintColor", issues);

        if (background.Opacity < 0.0 || background.Opacity > 1.0)
        {
            issues.Add(new ThemePresetValidationIssue(
                "background.opacity",
                ThemePresetValidationSeverity.Error,
                "Background opacity must be between 0.0 and 1.0."));
        }

        if (string.Equals(background.Mode, "solid", StringComparison.OrdinalIgnoreCase) &&
            string.IsNullOrWhiteSpace(background.PrimaryColor))
        {
            issues.Add(new ThemePresetValidationIssue(
                "background.primaryColor",
                ThemePresetValidationSeverity.Error,
                "Solid backgrounds require primaryColor."));
        }

        if (string.Equals(background.Mode, "gradient", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(background.PrimaryColor))
            {
                issues.Add(new ThemePresetValidationIssue(
                    "background.primaryColor",
                    ThemePresetValidationSeverity.Error,
                    "Gradient backgrounds require primaryColor."));
            }

            if (string.IsNullOrWhiteSpace(background.SecondaryColor))
            {
                issues.Add(new ThemePresetValidationIssue(
                    "background.secondaryColor",
                    ThemePresetValidationSeverity.Error,
                    "Gradient backgrounds require secondaryColor."));
            }
        }

        if (string.Equals(background.Mode, "image", StringComparison.OrdinalIgnoreCase) &&
            string.IsNullOrWhiteSpace(background.ImageUri))
        {
            issues.Add(new ThemePresetValidationIssue(
                "background.imageUri",
                ThemePresetValidationSeverity.Error,
                "Image backgrounds require imageUri."));
        }
    }

    private static void ValidateRequiredText(string? value, string path, List<ThemePresetValidationIssue> issues)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        issues.Add(new ThemePresetValidationIssue(
            path,
            ThemePresetValidationSeverity.Error,
            "Value is required."));
    }

    private static void ValidateOptionalColor(string? value, string path, List<ThemePresetValidationIssue> issues)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (IsHexColor(value))
        {
            return;
        }

        issues.Add(new ThemePresetValidationIssue(
            path,
            ThemePresetValidationSeverity.Error,
            "Value must be a hex color in #RRGGBB or #AARRGGBB format."));
    }

    private static bool IsHexColor(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || !ThemeColorPattern().IsMatch(value))
        {
            return false;
        }

        try
        {
            return ColorConverter.ConvertFromString(value) is Color;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
    private static partial Regex ThemeIdPattern();

    [GeneratedRegex("^#(?:[A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$", RegexOptions.CultureInvariant)]
    private static partial Regex ThemeColorPattern();
}