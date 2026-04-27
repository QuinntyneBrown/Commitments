---
id: 010
title: Dashboard grid + gridster-item still hard-code light theme colors
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: 837c9bd
flow: dashboard-layout
severity: high
---

# Dashboard grid + gridster-item still hard-code light theme colors

## Summary

`docs/ui-design.pen` → frame `Dashboard — LG (1280)` (id `OxYKj`)
shows the grid area on the same dark `--cui-bg` (`#121212`) the
shell uses, and each tile is the `Dashboard-Tile` component (id
`0UH2Q`) — `fill: #242424` (`--cui-surface-2`), `stroke: #3A3A3A`
(`--cui-divider`).

Today `dashboard-grid.component.scss` paints the grid area light
and wraps every tile in a bright white `gridster-item`:

```scss
.dashboard-grid {
  background: #f4f7fb;          /* light gutter behind tiles */
}
gridster-item {
  background: #ffffff;          /* white frame around the tile */
  border: 1px solid #d9e2ee;
  box-shadow: 0 10px 24px rgba(28, 43, 66, 0.08);
}
```

So even though the shell topbar and body are now dark (bug 009 +
bug 004), the entire grid area below the topbar still reads as a
light page — and every dark tile is surrounded by a chunky white
"card frame" the design never asks for.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Inspect `.dashboard-grid`'s background and any `gridster-item`'s
   background.

**Expected:** grid area = `rgb(18, 18, 18)` (`--cui-bg`); each
gridster-item = `rgb(36, 36, 36)` (`--cui-surface-2`) with the
`--cui-divider` stroke.
**Actual:** grid `rgb(244, 247, 251)` and gridster-item
`rgb(255, 255, 255)`.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

Edit `projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`:

  - `.dashboard-grid` — `background: var(--cui-bg)`
  - `gridster-item` — `background: var(--cui-surface-2)`,
                      `border: 1px solid var(--cui-divider)`,
                      `box-shadow: var(--cui-shadow-raised)`

Four token swaps in one SCSS file. Tile inner content is already
dark (its own component scss) — these tokens just stop the
wrapper / gutter from breaking the dark surface.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with a
`gridBackgroundColor()` helper and add a test in
`dashboard.spec.ts` asserting it equals `rgb(18, 18, 18)`.
Currently fails — it computes `rgb(244, 247, 251)`.
