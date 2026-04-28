---
id: bug-079
title: Dashboard tiles render with a double outer border because gridster-item is styled as a card
status: Fixed
---

# Bug 079 — Dashboard tiles have a double outer border

**Status**: Fixed

## Fix

`dashboard-grid.component.scss` no longer paints the `gridster-item`
host as a card. The default rule is reduced to
`overflow: hidden; background: transparent;`, dropping the 1px
divider border, the 8px radius, the surface-2 fill, and the 16px
inner padding that together produced the outer card. The
edit-mode override gains a matching `border-radius: 8px` so the
2px accent affordance hugs the tile-shell.

After the change, the only element on the ancestor chain between
`<body>` and the tile-shell that has a non-zero border or radius
is the `tile-shell` mat-card itself — confirmed via a runtime
DOM walk. The visible card now matches `Dashboard-Tile`
(`0UH2Q`).

A minor edit-mode side effect: the `tile-chrome` overlay
(DRAG badge + remove X) used to sit in the now-removed 16px
padding gutter at the top of `gridster-item`, so it now floats
over the tile-shell's top-left corner. That is a separate
cosmetic concern and should be tracked as its own bug if/when
the chrome needs to be repositioned.

## Description

Every dashboard tile renders inside two concentric "card"
rectangles in the running app:

1. The outer frame is the `gridster-item` host element that
   `angular-gridster2` injects to position each tile in the grid.
2. The inner frame is the `mat-card.tile-shell` rendered by
   `TileShellComponent`, which is the canonical `Dashboard-Tile`
   (`0UH2Q`) in `docs/ui-design.pen`.

The outer frame is created by
`dashboard-grid.component.scss`, which declares a card-like rule
on `gridster-item`:

```scss
gridster-item {
  overflow: hidden;
  padding: 16px;
  border: 1px solid var(--cui-divider);
  border-radius: 8px;
  background: var(--cui-surface-2);
}
```

This duplicates the chrome that the `tile-shell` mat-card already
provides (1px `--cui-divider` border, 8px radius,
`--cui-surface-2` background, internal padding, drop shadow). The
runtime DOM walk on a Daily Results tile shows both elements with
identical card properties:

- `gridster-item` — `border: 1px solid rgb(58,58,58); border-radius:
  8px; background: rgb(36,36,36); padding: 16px`
- `mat-card.tile-shell` — `border: 1px solid rgb(58,58,58);
  border-radius: 8px; background: rgb(36,36,36); box-shadow:
  rgba(0,0,0,0.6) 0 4px 12px`

`docs/ui-design.pen`'s Dashboard frames (`yZqDW` Live, `wk1pH`
Review, plus the per-size dashboard frames) place exactly one
`Dashboard-Tile` (`0UH2Q`) at each grid slot — there is no outer
container with its own border, radius, fill, or padding. Tile-to-tile
spacing is the gridster `margin` option (already set to `24` in
`DashboardLayoutStore.buildGridOptions`), not an internal pad.

`gridster-item` should be a transparent positioning slot. The
visible card is the responsibility of whatever component the tile
projects (here, `TileShellComponent`).

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`

## Reproduction

1. Run the dev server (`http://localhost:4200/`).
2. Land on the Dashboard route in Live mode.
3. Inspect any tile (e.g. Daily Results). The tile appears inside
   two nested card rectangles — an outer 1px `--cui-divider`
   border with 16px inner padding, and an inner `tile-shell`
   mat-card holding the actual content.
4. Compare to `docs/ui-design.pen` → `Dashboard Live — XL (1920)`
   (`yZqDW`) — only one card per tile.

## Expected

- `gridster-item` has no default border, no border-radius, no
  background, and no padding. It is a transparent slot.
- The visible tile is the `tile-shell` mat-card alone, matching
  `Dashboard-Tile` (`0UH2Q`) in the design.
- Edit mode keeps its accent-border affordance — but now applied
  to a tight rectangle around the tile-shell with matching 8px
  radius rather than around an extra padded frame.

## Verification

- Re-inspect a tile in the running app: the only element on the
  ancestor chain with a non-zero border between `body` and
  `tile-shell` is the `tile-shell` itself.
- Visual: a screenshot of the Dashboard at `http://localhost:4200/`
  shows tiles with a single card outline matching the design.
- Edit mode (toggle EDIT): each `gridster-item` shows only the
  2px accent border indicator, with rounded corners flush around
  the tile-shell.
