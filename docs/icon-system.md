# Icon System

The icon system gives every icon in your UI a **semantic role** (e.g. `action.close`, `status.warning`) rather than a hard-coded color or pack-specific name. The theme then resolves that role to the correct color, and an optional pack mapping resolves it to the correct icon identifier for whichever icon pack you're using.

## Architecture

```
icon role path              color role name     token path          rendered color
──────────────────────────  ──────────────────  ──────────────────  ────────────────
"action.delete"         →   "danger"        →   "accent.danger"  →  #E74C3C
"status.warning"        →   "warning"       →   "accent.warning" →  #F1C40F
"action.search"         →   "default"       →   "text.secondary" →  #C7D0D9
```

Each role path also has a **size** you can look up from `icon.size.*` in the icons file.

## `icons.json` Structure

```json
{
  "meta": {
    "themeId": "midnight",
    "version": "0.0.003"
  },
  "icon": {
    "size": { "xs": 12, "sm": 14, "md": 16, "lg": 20, "xl": 24 },
    "style": { "strokeWidth": 1.75, "corner": "soft" },
    "color": {
      "default":     "text.secondary",
      "muted":       "text.muted",
      "interactive": "accent.primary",
      "danger":      "accent.danger",
      "warning":     "accent.warning",
      "success":     "accent.success",
      "info":        "accent.info",
      "inverse":     "text.inverse"
    },
    "roles": {
      "action.add":      "interactive",
      "action.delete":   "danger",
      "status.warning":  "warning",
      "nav.settings":    "default"
    },
    "packs": {
      "lucide": {
        "url": "https://lucide.dev",
        "license": "ISC",
        "status": "license_verification_pending",
        "mapping": {
          "action.add":    "plus",
          "action.delete": "trash-2"
        }
      }
    }
  }
}
```

### `icon.size`

Named pixel sizes for icons. Use these keys when sizing icons in your UI rather than hardcoding numbers.

| Key | px | Use case |
|---|---|---|
| `xs` | 12 | Dense UI, badge decorations |
| `sm` | 14 | Compact lists, captions |
| `md` | 16 | Standard inline icons |
| `lg` | 20 | Card headers, action buttons |
| `xl` | 24 | Hero sections, empty states |

### `icon.style`

Optional visual style hints. These are advisory and not enforced by the validator.

| Key | Value | Meaning |
|---|---|---|
| `strokeWidth` | `1.75` | Recommended stroke width for outline icons |
| `corner` | `"soft"` | Corner style hint: `soft`, `round`, or `sharp` |

### `icon.color`

Maps **color role names** to token paths in `theme.tokens`. This is the bridge between the semantic icon role system and the raw color token layer.

```json
"color": {
  "default":     "text.secondary",
  "interactive": "accent.primary",
  "danger":      "accent.danger"
}
```

Color role names in the midnight theme:

| Color role | Token path | Resolved value |
|---|---|---|
| `default` | `text.secondary` | #C7D0D9 |
| `muted` | `text.muted` | #8A97A6 |
| `interactive` | `accent.primary` | #4DA3FF |
| `inverse` | `text.inverse` | #0B0F14 |
| `success` | `accent.success` | #2ECC71 |
| `warning` | `accent.warning` | #F1C40F |
| `danger` | `accent.danger` | #E74C3C |
| `info` | `accent.info` | #00C2FF |

### `icon.roles`

Maps **flat semantic role paths** to **color role names** from `icon.color`. Role paths use `group.name` dot notation.

```json
"roles": {
  "action.add":      "interactive",
  "action.delete":   "danger",
  "status.warning":  "warning",
  "nav.settings":    "default"
}
```

#### Role Groups

| Group | Purpose |
|---|---|
| `action` | User-triggered operations (add, delete, edit, search, …) |
| `nav` | Navigation elements (home, back, settings, sidebar, …) |
| `status` | Feedback/state indicators (info, success, warning, error) |
| `file` | File system representations (file, folder, image, code) |
| `window` | OS window chrome controls (minimize, maximize, close) |

### `icon.packs`

Optional. Maps role paths to pack-specific icon identifiers. Packs are interchangeable — your application code uses the semantic role, and the pack mapping resolves the identifier.

```json
"packs": {
  "lucide": {
    "url": "https://lucide.dev",
    "license": "ISC",
    "status": "license_verification_pending",
    "mapping": {
      "action.close":    "x",
      "action.add":      "plus",
      "status.warning":  "alert-triangle"
    }
  }
}
```

Pack `status` values:
- `license_verification_pending` — license has not been live-verified; do not redistribute
- `approved` — license verified for your use case
- `reference_only` — included for reference; verify before use in production

---

## Supported Icon Packs

| Pack | Identifier | URL | License |
|---|---|---|---|
| Lucide | `lucide` | https://lucide.dev | ISC |
| Heroicons | `heroicons` | https://heroicons.com | MIT |
| Tabler Icons | `tabler` | https://tabler-icons.io | MIT |
| Bootstrap Icons | `bootstrap-icons` | https://icons.getbootstrap.com | MIT |

> Licenses are unverified at time of writing. See `docs/third-party-references.md` and `references/licenses-to-verify.md`.

---

## Usage in Application Code

### Resolve the color for a role

```js
// 1. Load icons.json and theme.json for the active theme
// 2. Get the color role name
const colorRoleName = icons.icon.roles["action.delete"];  // → "danger"
// 3. Get the token path
const tokenPath = icons.icon.color[colorRoleName];        // → "accent.danger"
// 4. Look up the resolved hex value in the flat token map
const hex = flatTokens[tokenPath];                        // → "#E74C3C"
```

### Resolve the pack icon name for a role

```js
const iconName = icons.icon.packs.lucide.mapping["action.delete"]; // → "trash-2"
```

### Resolve the icon size

```js
const sizePx = icons.icon.size.md;  // → 16
```

### C++ (Win32)

```cpp
// Color by role
COLORREF deleteIconColor = theme.ResolveToken("accent.danger"); // #E74C3C

// Icon size
int iconSize = (int)iconSystem.GetSize("md"); // 16

// Pack identifier (for bitmap/SVG lookup)
std::string iconName = iconSystem.GetPackId("action.delete", "lucide"); // "trash-2"
```

### CSS

The generated `dist/css/midnight.css` exposes all token values. Reference the token that your role maps to:

```css
/* action.delete → danger → accent.danger */
.icon-delete { color: var(--accent-danger); /* #E74C3C */ }
```

---

## Adding a New Role

1. Add the role to `icon.roles` in `icons.json`:
   ```json
   "action.archive": "muted"
   ```
2. If needed, add pack mappings in `icon.packs.<packId>.mapping`:
   ```json
   "action.archive": "archive"
   ```
3. Run `npm run validate` to confirm the color role name exists in `icon.color`.

## Adding a New Color Role

1. Add a new entry to `icon.color` in `icons.json`:
   ```json
   "highlight": "accent.secondary"
   ```
2. Verify `accent.secondary` exists in `theme.tokens` — the validator will catch it if not.
3. Use the new color role in `icon.roles`:
   ```json
   "action.pin": "highlight"
   ```

## Authoring a New Theme

Your theme's `icons.json` must define all required fields in `icon.color`. The color role names themselves are arbitrary, but you must define every name that `icon.roles` references. The validator will report any role that maps to an undefined color name as an error.

Your theme must also define the token paths used in `icon.color` in `theme.tokens`. For example, if `icon.color.danger = "accent.danger"`, then `theme.tokens.accent.danger` must be defined.


## Concepts

| Concept | Description |
|---|---|
| **Icon role** | A semantic identifier for an icon's meaning, e.g. `action.close` or `status.warning` |
| **Color token** | A hex color defined in `theme.tokens.icon.*`, resolved from the role's `semantic` field |
| **Size token** | A numeric pixel value defined in `theme.scale.size.icon.*`, resolved from the role's `size` field |
| **Icon pack** | A named set of pack-specific identifiers (e.g. Lucide, Heroicons, Tabler) |

## icons.json Structure

```json
{
  "roles": {
    "<group>": {
      "<role>": {
        "semantic": "<icon token path>",
        "size": "<scale size path>"
      }
    }
  },
  "packs": {
    "<packId>": {
      "url": "...",
      "license": "...",
      "mapping": {
        "<group>.<role>": "<pack icon identifier>"
      }
    }
  }
}
```

### Role Groups

| Group | Purpose |
|---|---|
| `action` | User-triggered operations (close, add, edit, delete, …) |
| `nav` | Navigation elements (home, back, settings, sidebar, …) |
| `status` | Feedback/state indicators (info, success, warning, error) |
| `file` | File system representations (file, folder, image, code, …) |
| `window` | OS window chrome controls (minimize, maximize, close) |

### Semantic Field

The `semantic` field is a dot-notation path into `theme.tokens.icon.*`. For example:

```json
"semantic": "icon.default"   →   theme.tokens.icon.default  =  "#C7D0D9"
"semantic": "icon.warning"   →   theme.tokens.icon.warning  =  "#F1C40F"
"semantic": "icon.danger"    →   theme.tokens.icon.danger   =  "#E74C3C"
```

Available token paths for midnight dark theme:

| Token | Use case |
|---|---|
| `icon.default` | Neutral / default icons |
| `icon.interactive` | Icons on interactive controls (buttons, links) |
| `icon.muted` | De-emphasized / secondary icons |
| `icon.inverse` | Icons on dark-on-light surfaces |
| `icon.success` | Success/positive status icons |
| `icon.warning` | Warning/caution status icons |
| `icon.danger` | Danger/destructive/error icons |
| `icon.info` | Informational status icons |

### Size Field

The `size` field is a dot-notation path into `theme.scale.size.icon.*`:

| Token | px value | Use case |
|---|---|---|
| `size.icon.xs` | 12 | Dense UI, badges |
| `size.icon.sm` | 14 | Compact lists, captions |
| `size.icon.md` | 16 | Standard inline icons |
| `size.icon.lg` | 20 | Card headers, action buttons |
| `size.icon.xl` | 24 | Hero sections, empty states |

## Icon Packs

The `packs` section is optional. Each pack provides a `mapping` object from semantic role paths to pack-specific identifiers. Packs are interchangeable without changing application code.

### Supported Packs

| Pack | Identifier | License |
|---|---|---|
| [Lucide](https://lucide.dev) | `lucide` | ISC |
| [Heroicons](https://heroicons.com) | `heroicons` | MIT |
| [Tabler](https://tabler-icons.io) | `tabler` | MIT |

### Adding a New Pack

1. Add a new key under `packs` in `icons.json`
2. Add `url`, `license`, and a `mapping` object
3. Map each semantic role path (`"group.role"`) to the pack's icon identifier
4. Not every role needs a mapping — unmapped roles fall back to the `lucide` pack

```json
"packs": {
  "mypack": {
    "url": "https://example.com/icons",
    "license": "MIT",
    "mapping": {
      "action.close": "close-icon",
      "status.warning": "warning-triangle"
    }
  }
}
```

## Usage in Application Code

### C++ (Win32)

```cpp
// Resolve icon color from theme tokens
COLORREF iconColor = theme.ResolveToken("icon.default");

// Look up pack icon identifier
std::string iconId = iconSystem.Resolve("action.close", "lucide"); // → "X"

// Resolve icon size
int iconSize = (int)theme.ResolveScale("size.icon.md"); // → 16
```

### JSON Export

The built `dist/json/midnight.json` exposes all token values. For icon system integration, consume the `icon.*` keys from the `tokens` section.

### CSS

```css
/* Generated icon variables in dist/css/midnight.css */
--icon-default: #C7D0D9;
--icon-warning: #F1C40F;
--icon-danger: #E74C3C;
--size-icon-md: 16px;
```

## Authoring New Themes

When creating a new theme you must define all required `tokens.icon.*` entries:

```json
"icon": {
  "default":     "<hex>",
  "interactive": "<hex>",
  "muted":       "<hex>",
  "inverse":     "<hex>",
  "success":     "<hex>",
  "warning":     "<hex>",
  "danger":      "<hex>",
  "info":        "<hex>"
}
```

Run `npm run validate` to confirm all icon role references in `icons.json` resolve to valid tokens.
