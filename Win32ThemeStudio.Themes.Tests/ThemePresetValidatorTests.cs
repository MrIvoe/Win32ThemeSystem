using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.Themes.Tests;

[TestClass]
public class ThemePresetValidatorTests
{
    [TestMethod]
    public void Validate_ValidPreset_ReturnsNoIssues()
    {
        var preset = ThemePresetTestData.CreateValidPreset();

        var issues = ThemePresetValidator.Validate(preset);

        Assert.AreEqual(0, issues.Count);
    }

    [TestMethod]
    public void Validate_MissingRequiredPaletteKey_ReportsIssue()
    {
        var preset = ThemePresetTestData.CreateValidPreset(includeBackground: false);
        preset.PaletteValues.Remove(ThemePaletteKeys.Accent);

        var issues = ThemePresetValidator.Validate(preset);

        CollectionAssert.Contains(
            issues.Select(static issue => issue.Path).ToArray(),
            $"paletteValues.{ThemePaletteKeys.Accent}");
    }

    [TestMethod]
    public void Validate_InvalidImageBackground_ReportsIssue()
    {
        var preset = ThemePresetTestData.CreateValidPreset(includeBackground: false);
        preset = new ThemePreset
        {
            FormatVersion = preset.FormatVersion,
            Theme = preset.Theme,
            PaletteValues = preset.PaletteValues,
            Background = new ThemeBackgroundPreset
            {
                Mode = "image",
                SizingMode = "fill",
                TintColor = "#40171C23",
                Opacity = 0.75
            }
        };

        var issues = ThemePresetValidator.Validate(preset);

        CollectionAssert.Contains(
            issues.Select(static issue => issue.Path).ToArray(),
            "background.imageUri");
    }

    [TestMethod]
    public void EnsureValid_InvalidPreset_ThrowsActionableMessage()
    {
        var preset = ThemePresetTestData.CreateValidPreset();
        preset = new ThemePreset
        {
            FormatVersion = "2.0",
            Theme = new ThemePresetDescriptor
            {
                Id = "Signal Night",
                DisplayName = string.Empty,
                Appearance = preset.Theme.Appearance,
                Category = preset.Theme.Category,
                AccentFamily = preset.Theme.AccentFamily,
                Description = preset.Theme.Description,
                Tags = preset.Theme.Tags,
                SourceThemeUri = preset.Theme.SourceThemeUri
            },
            Background = preset.Background,
            PaletteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [ThemePaletteKeys.Background] = "not-a-color"
            }
        };

        var exception = Assert.ThrowsException<InvalidOperationException>(() => ThemePresetValidator.EnsureValid(preset));

        StringAssert.Contains(exception.Message, "formatVersion");
        StringAssert.Contains(exception.Message, "theme.id");
        StringAssert.Contains(exception.Message, "paletteValues.Brush.Background");
    }
}