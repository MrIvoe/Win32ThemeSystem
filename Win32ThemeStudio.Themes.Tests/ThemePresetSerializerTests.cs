using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win32ThemeStudio.Themes;

namespace Win32ThemeStudio.Themes.Tests;

[TestClass]
public class ThemePresetSerializerTests
{
    [TestMethod]
    public void SerializeDeserialize_PreservesBackgroundMetadata()
    {
        var preset = ThemePresetTestData.CreateValidPreset();

        var json = ThemePresetSerializer.Serialize(preset);
        var roundTrip = ThemePresetSerializer.Deserialize(json);

        Assert.IsNotNull(roundTrip.Background);
        Assert.AreEqual("gradient", roundTrip.Background.Mode);
        Assert.AreEqual("#FF13171C", roundTrip.Background.PrimaryColor);
        Assert.AreEqual("#FF1D232C", roundTrip.Background.SecondaryColor);
        Assert.AreEqual("fill", roundTrip.Background.SizingMode);
        Assert.AreEqual("#40171C23", roundTrip.Background.TintColor);
        Assert.AreEqual(0.92, roundTrip.Background.Opacity, 0.0001);
        Assert.IsTrue(roundTrip.Background.BlurEnabled);
    }

    [TestMethod]
    public void Deserialize_WithoutBackground_RemainsSupported()
    {
                var json = ThemePresetSerializer.Serialize(ThemePresetTestData.CreateValidPreset(includeBackground: false));

        var preset = ThemePresetSerializer.Deserialize(json);

        Assert.IsNotNull(preset);
        Assert.IsNull(preset.Background);
        Assert.AreEqual("signal-night", preset.Theme.Id);
        Assert.AreEqual("#FF13171C", preset.PaletteValues["Brush.Background"]);
    }
}
