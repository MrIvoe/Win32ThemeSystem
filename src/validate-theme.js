const fs = require("node:fs");
const path = require("node:path");
const { flattenTokens, flattenSemantic } = require("./resolve-token-path");

const REQUIRED_TOKEN_GROUPS = [
  "background",
  "surface",
  "text",
  "accent",
  "border",
  "state",
  "syntax"
];

// Scale groups in theme.json that hold non-color values (numbers/strings).
// These are validated separately and are not subject to the hex-color check.
const SCALE_COLOR_EXEMPT_GROUPS = new Set([
  "size",
  "space",
  "radius",
  "border",
  "font",
  "shadow",
  "motion",
  "opacity",
  "layer"
]);

const HEX_COLOR_PATTERN = /^#(?:[A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$/;
const TOKEN_REF_PATTERN = /^[a-z][a-z0-9_]*(\.[a-z][a-z0-9_]*)+$/;

function readJson(filePath) {
  const raw = fs.readFileSync(filePath, "utf8");
  return JSON.parse(raw);
}

function assert(condition, message, errors) {
  if (!condition) {
    errors.push(message);
  }
}

function validateTheme(theme) {
  const errors = [];

  assert(theme && typeof theme === "object", "Theme must be an object", errors);
  if (errors.length > 0) {
    return errors;
  }

  assert(theme.meta && typeof theme.meta === "object", "Missing meta object", errors);
  assert(theme.tokens && typeof theme.tokens === "object", "Missing tokens object", errors);

  if (theme.meta) {
    assert(typeof theme.meta.id === "string" && /^[a-z0-9]+(?:-[a-z0-9]+)*$/.test(theme.meta.id), "meta.id must be kebab-case", errors);
    assert(typeof theme.meta.name === "string" && theme.meta.name.length > 0, "meta.name is required", errors);
    assert(typeof theme.meta.version === "string" && /^[0-9]+\.[0-9]+\.[0-9]+$/.test(theme.meta.version), "meta.version must match x.y.z", errors);
    assert(theme.meta.mode === "light" || theme.meta.mode === "dark", "meta.mode must be light or dark", errors);
  }

  if (theme.tokens) {
    for (const group of REQUIRED_TOKEN_GROUPS) {
      assert(theme.tokens[group] && typeof theme.tokens[group] === "object", `Missing required token group: ${group}`, errors);
    }

    const flatTokens = flattenTokens(theme.tokens);
    for (const [tokenPath, value] of Object.entries(flatTokens)) {
      assert(typeof value === "string", `Token ${tokenPath} must be a string`, errors);
      assert(HEX_COLOR_PATTERN.test(String(value)), `Token ${tokenPath} must be a valid hex color`, errors);
    }
  }

  return errors;
}

function validateSemantic(semantic, theme) {
  const errors = [];

  assert(semantic && typeof semantic === "object", "Semantic file must be an object", errors);
  if (!semantic || typeof semantic !== "object") {
    return errors;
  }

  const flatSemantic = flattenSemantic(semantic);
  const flatTokens = flattenTokens(theme.tokens || {});

  for (const [semanticPath, tokenPath] of Object.entries(flatSemantic)) {
    assert(typeof tokenPath === "string", `Semantic path ${semanticPath} must map to a token path string`, errors);
    if (typeof tokenPath === "string") {
      assert(Boolean(flatTokens[tokenPath]), `Semantic path ${semanticPath} points to missing token ${tokenPath}`, errors);
    }
  }

  return errors;
}

function validateComponents(components, scale) {
  const errors = [];

  assert(components && typeof components === "object", "Components file must be an object", errors);
  if (!components || typeof components !== "object") {
    return errors;
  }

  const flatScale = flattenTokens(scale || {});

  function walkNode(node, nodePath) {
    if (node === null || node === undefined) {
      errors.push(`Component node at ${nodePath} is null/undefined`);
      return;
    }
    if (typeof node === "object" && !Array.isArray(node)) {
      for (const [key, value] of Object.entries(node)) {
        walkNode(value, `${nodePath}.${key}`);
      }
      return;
    }
    // Scalar: must be a number, or a token ref that exists in scale
    if (typeof node === "number") {
      return; // raw number value is allowed
    }
    if (typeof node === "string") {
      if (TOKEN_REF_PATTERN.test(node)) {
        assert(Boolean(flatScale[node]), `Component token ref '${nodePath}' points to missing scale key '${node}'`, errors);
      }
      // plain string (non-ref) is allowed (e.g. a CSS value)
      return;
    }
    errors.push(`Component node at ${nodePath} has unexpected type: ${typeof node}`);
  }

  for (const [key, value] of Object.entries(components)) {
    walkNode(value, key);
  }

  return errors;
}

function validateIcons(icons, theme) {
  const errors = [];

  assert(icons && typeof icons === "object", "Icons file must be an object", errors);
  if (!icons || typeof icons !== "object") {
    return errors;
  }

  assert(icons.roles && typeof icons.roles === "object", "Icons file must have a 'roles' object", errors);

  if (!icons.roles) {
    return errors;
  }

  const flatTokens = flattenTokens(theme.tokens || {});
  const flatScale = flattenTokens(theme.scale || {});

  for (const [group, groupValue] of Object.entries(icons.roles)) {
    if (!groupValue || typeof groupValue !== "object") {
      errors.push(`Icon role group '${group}' must be an object`);
      continue;
    }
    for (const [role, descriptor] of Object.entries(groupValue)) {
      const rolePath = `${group}.${role}`;
      assert(descriptor && typeof descriptor === "object", `Icon role '${rolePath}' must be an object with semantic and size fields`, errors);
      if (!descriptor || typeof descriptor !== "object") {
        continue;
      }
      assert(typeof descriptor.semantic === "string" && TOKEN_REF_PATTERN.test(descriptor.semantic),
        `Icon role '${rolePath}'.semantic must be a dot-notation token path`, errors);
      assert(typeof descriptor.size === "string" && TOKEN_REF_PATTERN.test(descriptor.size),
        `Icon role '${rolePath}'.size must be a dot-notation scale path`, errors);
      if (typeof descriptor.semantic === "string") {
        assert(Boolean(flatTokens[descriptor.semantic]),
          `Icon role '${rolePath}'.semantic '${descriptor.semantic}' not found in theme tokens`, errors);
      }
      if (typeof descriptor.size === "string") {
        assert(Boolean(flatScale[descriptor.size]),
          `Icon role '${rolePath}'.size '${descriptor.size}' not found in theme scale`, errors);
      }
    }
  }

  return errors;
}

function validateThemePair(themePath, semanticPath, componentsPath, iconsPath) {
  const theme = readJson(themePath);
  const semantic = readJson(semanticPath);

  const errors = [
    ...validateTheme(theme),
    ...validateSemantic(semantic, theme)
  ];

  if (componentsPath) {
    const components = readJson(componentsPath);
    errors.push(...validateComponents(components, theme.scale));
  }

  if (iconsPath) {
    const icons = readJson(iconsPath);
    errors.push(...validateIcons(icons, theme));
  }

  return {
    valid: errors.length === 0,
    errors,
    theme,
    semantic
  };
}

function main() {
  const themePath = process.argv[2];
  const semanticPath = process.argv[3];

  if (!themePath || !semanticPath) {
    console.error("Usage: node src/validate-theme.js <theme.json> <semantic.json> [components.json] [icons.json]");
    process.exit(2);
  }

  const absoluteThemePath = path.resolve(themePath);
  const absoluteSemanticPath = path.resolve(semanticPath);
  const absoluteComponentsPath = process.argv[4] ? path.resolve(process.argv[4]) : undefined;
  const absoluteIconsPath = process.argv[5] ? path.resolve(process.argv[5]) : undefined;

  const result = validateThemePair(absoluteThemePath, absoluteSemanticPath, absoluteComponentsPath, absoluteIconsPath);
  if (!result.valid) {
    for (const error of result.errors) {
      console.error(`- ${error}`);
    }
    process.exit(1);
  }

  console.log(`Validation passed for ${absoluteThemePath} and ${absoluteSemanticPath}`);
}

if (require.main === module) {
  main();
}

module.exports = {
  validateTheme,
  validateSemantic,
  validateComponents,
  validateIcons,
  validateThemePair
};
