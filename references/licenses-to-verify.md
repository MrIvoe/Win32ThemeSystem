# Licenses To Verify

This file tracks third-party sources referenced in the Themes repo that require license verification before any public release or redistribution.

Verification is needed when:
- Publishing the package publicly (removing `"private": true` from package.json)
- Embedding pack-specific icon names/identifiers in released artifacts
- Including attribution notices
- Generating adapter outputs intended for redistribution

---

## Icon Packs — Pending Verification

| ID | Name | Claimed License | URL | Notes |
|---|---|---|---|---|
| lucide | Lucide | ISC | https://lucide.dev | Verify ISC license still applies to all icons in current version |
| heroicons | Heroicons | MIT | https://heroicons.com | Verify MIT license applies to all variants (outline + solid) |
| tabler | Tabler Icons | MIT | https://tabler-icons.io | Verify MIT; check if attribution is required |
| bootstrap-icons | Bootstrap Icons | MIT | https://icons.getbootstrap.com | Verify MIT; check if it covers SVG exports and embedding |
| feather | Feather Icons | MIT | https://feathericons.com | Verify project is still maintained and license is unchanged |
| material-symbols | Material Symbols | Apache-2.0 | https://fonts.google.com/icons | Apache-2.0 typically requires attribution; verify exact terms |
| font-awesome-free | Font Awesome Free | CC BY 4.0 / MIT / SIL OFL 1.1 | https://fontawesome.com | Split license; verify which applies to which assets by category |

### Verification checklist per pack

- [ ] Confirm license is still current (check project repo/releases)
- [ ] Confirm license allows use in software projects without source redistribution
- [ ] Confirm whether attribution is required in derived works
- [ ] Confirm whether embedding icon identifiers (names) in a mapping file is covered
- [ ] Note any changes to the license since this entry was written

---

## UI Libraries — Verification Status

| ID | Name | Claimed License | URL | Status |
|---|---|---|---|---|
| material | Material Design | Apache-2.0 | https://m3.material.io | reference_only — no code copied |
| fluent | Fluent Design System | MIT | https://fluent2.microsoft.design | reference_only — no code copied |
| bootstrap | Bootstrap | MIT | https://getbootstrap.com | reference_only — no code copied |
| tabler-ui | Tabler UI | MIT | https://tabler.io | reference_only — no code copied |
| shadcn | shadcn/ui | MIT | https://ui.shadcn.com | reference_only — no code copied |
| radix | Radix Primitives | MIT | https://www.radix-ui.com | reference_only — no code copied |
| ant-design | Ant Design | MIT | https://ant.design | reference_only — no code copied |

For `reference_only` entries, no code or assets are directly embedded. The reference is used only for:
- naming conventions
- component state pattern ideas
- control behavior reference

---

## Policy Reminder

> Third-party names, logos, icon sets, and design system assets referenced in this repo are the property of their respective owners. References do not imply endorsement. All third-party sources are used for inspiration and optional compatibility mapping only. No third-party assets are embedded in the core theme source files.
