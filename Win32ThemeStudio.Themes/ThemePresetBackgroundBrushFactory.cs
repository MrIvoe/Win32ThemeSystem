using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Win32ThemeStudio.Themes;

public static class ThemePresetBackgroundBrushFactory
{
    public static Brush CreateBrush(ThemeBackgroundPreset background, string fallbackColorHex)
    {
        ArgumentNullException.ThrowIfNull(background);

        var normalized = background.Normalize();
        var fallbackColor = ParseColorOrFallback(fallbackColorHex, Colors.Black);

        Brush brush = normalized.Mode switch
        {
            "gradient" => CreateGradientBrush(normalized, fallbackColor),
            "image" => CreateImageBrush(normalized, fallbackColor),
            _ => CreateSolidBrush(normalized.PrimaryColor, fallbackColor)
        };

        brush.Opacity = normalized.Opacity;
        if (brush.CanFreeze)
        {
            brush.Freeze();
        }

        return brush;
    }

    private static Brush CreateGradientBrush(ThemeBackgroundPreset background, Color fallbackColor)
    {
        var primary = ParseColorOrFallback(background.PrimaryColor, fallbackColor);
        var secondary = ParseColorOrFallback(background.SecondaryColor, primary);

        return new LinearGradientBrush(primary, secondary, 135.0);
    }

    private static Brush CreateImageBrush(ThemeBackgroundPreset background, Color fallbackColor)
    {
        if (string.IsNullOrWhiteSpace(background.ImageUri))
        {
            return CreateSolidBrush(background.PrimaryColor, fallbackColor);
        }

        try
        {
            var imageUri = Uri.TryCreate(background.ImageUri, UriKind.RelativeOrAbsolute, out var parsedUri)
                ? parsedUri
                : throw new InvalidOperationException("Image URI is invalid.");

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            var brush = new ImageBrush(bitmap)
            {
                Stretch = ParseStretch(background.SizingMode),
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center
            };

            if (string.Equals(background.SizingMode, "tile", StringComparison.OrdinalIgnoreCase))
            {
                brush.TileMode = TileMode.Tile;
                brush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                brush.Viewport = new System.Windows.Rect(0, 0, 0.25, 0.25);
            }

            return brush;
        }
        catch (Exception)
        {
            return CreateSolidBrush(background.PrimaryColor, fallbackColor);
        }
    }

    private static Brush CreateSolidBrush(string? primaryColor, Color fallbackColor)
    {
        return new SolidColorBrush(ParseColorOrFallback(primaryColor, fallbackColor));
    }

    private static Color ParseColorOrFallback(string? candidateColor, Color fallbackColor)
    {
        if (string.IsNullOrWhiteSpace(candidateColor))
        {
            return fallbackColor;
        }

        return ColorConverter.ConvertFromString(candidateColor) is Color color
            ? color
            : fallbackColor;
    }

    private static Stretch ParseStretch(string? sizingMode)
    {
        if (string.IsNullOrWhiteSpace(sizingMode))
        {
            return Stretch.Fill;
        }

        return sizingMode.Trim().ToLowerInvariant() switch
        {
            "fit" => Stretch.Uniform,
            "stretch" => Stretch.Fill,
            "tile" => Stretch.None,
            "center" => Stretch.None,
            _ => Stretch.UniformToFill
        };
    }
}