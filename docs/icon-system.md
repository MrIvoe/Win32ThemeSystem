# Icon System

The Themes icon system defines a semantic layer between visual icon packs and application code. Instead of hardcoding pack-specific icon identifiers, your application references semantic role names. The theme then maps roles to both the correct color token and the appropriate icon from whichever pack is configured.

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
