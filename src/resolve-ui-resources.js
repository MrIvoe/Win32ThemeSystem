const fs = require("node:fs");
const path = require("node:path");

function readJson(filePath) {
  return JSON.parse(fs.readFileSync(filePath, "utf8"));
}

function hasPath(target, dotPath) {
  if (!target || typeof target !== "object" || typeof dotPath !== "string") {
    return false;
  }
  const segments = dotPath.split(".");
  let cursor = target;
  for (const segment of segments) {
    if (!cursor || typeof cursor !== "object" || !(segment in cursor)) {
      return false;
    }
    cursor = cursor[segment];
  }
  return true;
}

function getPath(target, dotPath) {
  const segments = dotPath.split(".");
  let cursor = target;
  for (const segment of segments) {
    cursor = cursor[segment];
  }
  return cursor;
}

function loadThemeArtifacts(themeDir) {
  return {
    theme: readJson(path.join(themeDir, "theme.json")),
    components: readJson(path.join(themeDir, "components.json")),
    icons: readJson(path.join(themeDir, "icons.json")),
    resources: readJson(path.join(themeDir, "resources.json"))
  };
}

function resolveUiResources(themeDir, selection = {}) {
  const { theme, components, icons, resources } = loadThemeArtifacts(themeDir);

  const iconSelection = selection.iconPack || resources.ui.icons.defaultPack;
  const styleSelection = selection.buttonStyle || resources.ui.buttons.defaultStyle;

  const styleConfig = resources.ui.buttons.styles[styleSelection];
  if (!styleConfig) {
    throw new Error(`Unknown button style: ${styleSelection}`);
  }

  if (!(iconSelection in (icons.icon.packs || {}))) {
    throw new Error(`Unknown icon pack: ${iconSelection}`);
  }

  if (!hasPath(components, styleConfig.componentRef)) {
    throw new Error(`Component ref '${styleConfig.componentRef}' is missing in components.json`);
  }

  const resolvedButtonTokens = getPath(components, styleConfig.componentRef);
  const effectivePack = styleConfig.iconPackOverride || iconSelection;
  const packData = icons.icon.packs[effectivePack] || {};

  return {
    meta: {
      themeId: resources.meta.themeId || theme.meta.id,
      version: resources.meta.version || theme.meta.version
    },
    selection: {
      iconPack: iconSelection,
      buttonStyle: styleSelection,
      effectiveIconPack: effectivePack
    },
    button: {
      styleKey: styleSelection,
      label: styleConfig.label,
      componentRef: styleConfig.componentRef,
      resolvedRefs: resolvedButtonTokens
    },
    icons: {
      pack: effectivePack,
      availablePacks: resources.ui.icons.availablePacks,
      style: icons.icon.style || {},
      size: icons.icon.size,
      colorRoles: icons.icon.color,
      roleColors: icons.icon.roles,
      roleToPackIcon: packData.mapping || {}
    },
    families: resources.ui.buttons.families,
    controls: resources.ui.controls || {}
  };
}

function parseArgs(argv) {
  const args = {
    themeDir: argv[2],
    iconPack: undefined,
    buttonStyle: undefined
  };

  for (let i = 3; i < argv.length; i += 1) {
    const token = argv[i];
    if (token === "--icon-pack") {
      args.iconPack = argv[i + 1];
      i += 1;
      continue;
    }
    if (token === "--button-style") {
      args.buttonStyle = argv[i + 1];
      i += 1;
      continue;
    }
  }

  return args;
}

function main() {
  const args = parseArgs(process.argv);
  if (!args.themeDir) {
    console.error("Usage: node src/resolve-ui-resources.js <theme-dir> [--icon-pack <pack>] [--button-style <style>]");
    process.exit(2);
  }

  const themeDir = path.resolve(args.themeDir);
  const resolved = resolveUiResources(themeDir, {
    iconPack: args.iconPack,
    buttonStyle: args.buttonStyle
  });

  console.log(JSON.stringify(resolved, null, 2));
}

if (require.main === module) {
  main();
}

module.exports = {
  resolveUiResources
};
