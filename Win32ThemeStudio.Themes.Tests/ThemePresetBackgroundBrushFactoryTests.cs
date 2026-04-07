using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.Themes.Tests;

[TestClass]
public class ThemePresetBackgroundBrushFactoryTests
{
    [TestMethod]
    public void CreateBrush_GradientMode_ReturnsGradientBrush()
    {
        var background = new ThemeBackgroundPreset
        {
            Mode = "gradient",
            PrimaryColor = "#FF101820",
            SecondaryColor = "#FF1F2A36",
            Opacity = 0.9
        };

        var brush = ThemePresetBackgroundBrushFactory.CreateBrush(background, "#FF000000");

        Assert.IsInstanceOfType<LinearGradientBrush>(brush);
        Assert.AreEqual(0.9, brush.Opacity, 0.0001);
    }

    [TestMethod]
    public void CreateBrush_SolidModeWithoutPrimary_UsesFallbackColor()
    {
        var background = new ThemeBackgroundPreset
        {
            Mode = "solid"
        };

        var brush = ThemePresetBackgroundBrushFactory.CreateBrush(background, "#FF334455");

        Assert.IsInstanceOfType<SolidColorBrush>(brush);
        var solidBrush = (SolidColorBrush)brush;
        Assert.AreEqual((Color)ColorConverter.ConvertFromString("#FF334455"), solidBrush.Color);
    }

    [TestMethod]
    public void CreateBrush_ImageModeWithoutImageUri_FallsBackToSolid()
    {
        var background = new ThemeBackgroundPreset
        {
            Mode = "image",
            PrimaryColor = "#FF221144"
        };

        var brush = ThemePresetBackgroundBrushFactory.CreateBrush(background, "#FF000000");

        Assert.IsInstanceOfType<SolidColorBrush>(brush);
        var solidBrush = (SolidColorBrush)brush;
        Assert.AreEqual((Color)ColorConverter.ConvertFromString("#FF221144"), solidBrush.Color);
    }
}