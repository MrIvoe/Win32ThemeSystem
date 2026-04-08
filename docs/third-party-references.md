# Third-Party References

`@mrivoe/themes` references several external icon packs and UI design systems for inspiration, naming conventions, and optional compatibility mapping. This document defines the policy governing those references and lists each source with its current status.

---

## Policy

### What references are used for

External libraries are used as:

- **Inspiration** — component state models, token naming conventions, role hierarchies
- **Naming bridges** — mapping our semantic roles to pack-specific icon identifiers
- **Behavior references** — understanding expected control states (hover, focus, active, disabled, checked, etc.)
- **Compatibility targets** — optional adapters that map our roles to pack-specific names so consumers can use their preferred icon pack

### What references are NOT used for

- Third-party code or assets are **not embedded** in core theme source files (`theme.json`, `semantic.json`, `components.json`, `icons.json`)
- Third-party names, logos, or proprietary assets are **not redistributed**
- No framework-specific class names or design system component names are used in source tokens

### Naming rules

| Bad | Good | Reason |
|---|---|---|
| `bootstrapPrimaryButton` | `button.primary.bg` | Framework-agnostic |
| `heroiconBlue` | `icon.interactive` | Semantic, not pack-specific |
| `materialDialogBg` | `dialog.surface.bg` | Role-based, not system-bound |
| `fluentCard` | `card.default.bg` | Universal component path |

---

## Icon Pack References

Icon packs are referenced in `icons.json` under `icon.packs.*`. These mappings exist so that consumers can look up which icon to render from their preferred pack for a given semantic role. The mappings contain only the pack's own icon identifiers (names) — no SVG paths or bitmap assets are copied.

All icon pack entries in this repo should be treated as **reference_only** or **license_verification_pending** until explicitly verified and marked as `approved` in `references/licenses-to-verify.md`.

| Pack | URL | Claimed License | Status |
|---|---|---|---|
| Lucide | https://lucide.dev | ISC | license_verification_pending |
| Heroicons | https://heroicons.com | MIT | license_verification_pending |
| Tabler Icons | https://tabler-icons.io | MIT | license_verification_pending |
| Bootstrap Icons | https://icons.getbootstrap.com | MIT | license_verification_pending |
| Feather Icons | https://feathericons.com | MIT | license_verification_pending |
| Material Symbols | https://fonts.google.com/icons | Apache-2.0 | license_verification_pending |
| Font Awesome Free | https://fontawesome.com | CC BY 4.0 / MIT / SIL OFL 1.1 | license_verification_pending |

> **Note:** Licenses listed above are based on prior knowledge and have not been live-verified at the time of writing. Verify each before any public distribution. See `references/licenses-to-verify.md` for the full verification checklist.

---

## UI Library References

These libraries and design systems are referenced purely for design inspiration and component modeling. No code or assets from these projects are included in this repository.

| Library | URL | Type | Status |
|---|---|---|---|
| Material Design (M3) | https://m3.material.io | ui-system | reference_only |
| Fluent Design System | https://fluent2.microsoft.design | ui-system | reference_only |
| Bootstrap | https://getbootstrap.com | ui-library | reference_only |
| Tabler UI | https://tabler.io | ui-library | reference_only |
| shadcn/ui | https://ui.shadcn.com | ui-library | reference_only |
| Radix Primitives | https://www.radix-ui.com | ui-library | reference_only |
| Ant Design | https://ant.design | ui-library | reference_only |

---

## Before Public Release

Before removing `"private": true` from `package.json` or publishing the package:

1. Review `references/licenses-to-verify.md` and complete all pending items
2. Confirm each referenced icon pack's license permits use in your distribution context
3. Add appropriate attribution notices if required (Apache-2.0 requires attribution; CC BY 4.0 requires attribution)
4. Update status fields in `references/icon-packs.json` and `references/ui-libraries.json` from `license_verification_pending` to `approved` or `excluded`
5. Consider consulting a legal resource for any split or complex licenses (Font Awesome Free, Material Symbols)

---

## Adding New References

When adding a new icon pack or UI library reference:

1. Add an entry to the appropriate file in `references/`
2. Set `status` to `license_verification_pending` unless the license has been verified
3. Add a row to `references/licenses-to-verify.md` with the verification checklist
4. Do not add pack mappings to `icons.json` until the license is confirmed to allow it
5. Do not copy code, SVGs, or design assets into this repository

---

## Disclaimer

All third-party names, trademarks, logos, and design systems referenced in this repository are the property of their respective owners. References in this repository do not imply endorsement, partnership, or affiliation.
