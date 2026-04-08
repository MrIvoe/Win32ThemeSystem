# Changelog

## 0.0.003

- Upgraded midnight theme to full universal design token platform with non-color scale tokens (size, space, radius, border, font, shadow, motion, opacity, layer).
- Added `tokens.icon` group with 8 semantic icon color roles.
- Added `tokens.state.selected` and `tokens.state.error` to state token group.
- Expanded semantic.json from 4 control groups to ~30: button variants (primary/secondary/danger/ghost), input, checkbox, radio, toggle, select, dropdown, tab, menu, tooltip, card, panel, modal, alert, badge, table, tree, scrollbar, editor, titlebar, sidebar, settings, tray, icon, plugin.
- Added `themes/midnight/components.json` — structural component tokens with size/spacing/radius/font refs.
- Added `themes/midnight/icons.json` — semantic icon role system with Lucide, Heroicons, Tabler pack mappings.
- Added `schema/components.schema.json` and `schema/icons.schema.json`.
- Updated `src/validate-theme.js` to validate components and icons files in addition to theme/semantic pair; accepts `[components.json] [icons.json]` as optional CLI args.
- Updated `src/build-all.js` to auto-detect and validate components/icons when present.
- Added `docs/icon-system.md` — full icon role authoring guide with pack compat table.
- Added `docs/component-token-guide.md` — component structural token reference and usage guide.
- Updated `package.json` version to `0.0.003`.
- Added 10 total adapter outputs: CSS, JSON, Python, C, C++, Java, SCSS, Tailwind, Qt, Tkinter.
- Published cross-repo theme contract documentation with canonical namespaces, compatibility versioning rules, fallback behavior, and consumer error-handling expectations.

## 0.0.002

- Reworked Themes into a universal token-first architecture with three layers: raw tokens, semantic mapping, and adapters.
- Added starter universal theme at themes/midnight (theme.json + semantic.json).
- Added schema contracts for universal tokens and semantic references.
- Added token resolver and validation utilities in src/.
- Added exporters for CSS, JSON, Python, C, and C++ plus build pipeline output into dist/.
- Updated repository documentation for token naming rules, adapter separation, and single-source export flow.

## 0.0.001

- Added required docs scaffolding for theme-platform roadmap alignment.
- Established canonical documentation entrypoints for schema, authoring, backgrounds, import, and fallback behavior.
- Added machine-readable preset schema (`theme-preset.schema.json`) and sample preset artifact for import/export alignment.
- Added optional background metadata support to the preset model and schema (`background` block, preserved on round-trip).
- Added serializer round-trip tests covering optional background metadata and backward-compatible presets without background data.
- Added runtime preset validation with structured diagnostics and bootstrapper-side import gating before theme activation.
- Added canonical `ThemeManager` helper APIs for validated preset JSON/file import and one-step initialize-from-import flows.
- Added `ThemePresetBackgroundBrushFactory` and bootstrapper runtime rendering for optional preset background metadata.
- Added default background metadata generation for built-in theme exports and demo parity for shared preset background rendering.

## Notes

- Repository remains in unstable track while platform interfaces continue to mature.
