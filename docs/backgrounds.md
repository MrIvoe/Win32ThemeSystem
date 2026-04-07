# Backgrounds

Current status:

- background configuration now has an optional preset-schema layer via the `background` object
- current implementation preserves background metadata during preset serialize/deserialize round-trips
- runtime consumption now includes bootstrapper sample rendering via `ThemePresetBackgroundBrushFactory`

Background model targets:

- solid color
- gradient
- image
- fit/fill/stretch/tile behavior
- tint and opacity
- optional blur/transparency where supported

Current runtime rendering behavior:

- `solid` mode renders a solid color brush
- `gradient` mode renders a linear gradient brush
- `image` mode attempts to load an image brush and falls back to solid if the source is unavailable
- `sizingMode` maps to WPF stretch/tile behavior (`fill`, `fit`, `stretch`, `tile`, `center`)
- `opacity` is applied directly on the produced brush

All background settings should support global defaults and per-fence override compatibility.
