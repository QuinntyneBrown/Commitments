---
id: 048
title: styles.scss uses deprecated @import for SCSS partials - swap to @use for Dart Sass 3.0 forward-compat
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 276ec94
flow: dashboard-layout
severity: low
---

# styles.scss uses deprecated @import for SCSS partials — swap to @use for Dart Sass 3.0 forward-compat

## Summary

`npm run build` emits a Dart Sass deprecation warning every
build:

```
[WARNING] Deprecation [plugin angular-sass]
  projects/commitments-app/src/styles.scss:5:8:
  5 │ @import '../../commitments-ui/src/lib/tokens/tokens';
  Sass @import rules are deprecated and will be removed in
  Dart Sass 3.0.0.
```

Both partials (`./styles/theme.scss` and
`../../commitments-ui/src/lib/tokens/tokens`) only emit CSS
rules / `:root` variables — no Sass-level `$variables` or
`@mixin` declarations are consumed by `styles.scss`, so the
`@use` namespace doesn't need any adjustments at the call
site.

## Reproduction

```
cd frontend
npm run build
```

…emits the warning twice (once per `@import` line).

## Fix outline (radically simple)

In `projects/commitments-app/src/styles.scss`:

  - `@import './styles/theme.scss';` →
    `@use './styles/theme.scss';`
  - `@import '../../commitments-ui/src/lib/tokens/tokens';` →
    `@use '../../commitments-ui/src/lib/tokens/tokens';`

Two literal swaps. The remaining two `@import` lines target
**CSS** files (`ag-grid-community/styles/ag-grid.css` and
`ag-grid-community/styles/ag-theme-material.css`) — those stay
on `@import` because `@use` doesn't accept `.css` paths.

## Tests to add (failing first)

The build warning itself is the artefact: pre-fix
`npm run build` emits two deprecation warnings on lines 2 and
5 of `styles.scss`; post-fix neither warning fires.
