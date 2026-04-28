---
id: bug-048
title: Tile-shell icon hard-codes `var(--cui-primary)`; three tile designs (monthly-progress, outstanding-todos, consistency-trend) specify themed icon colours
status: Open
---

# Bug 048 — Tile-shell icon colour is fixed at `--cui-primary` instead of per-tile-design

**Status**: Open

## Description

Tile icons in the .pen designs vary by tile theme:

| Tile                | Icon            | .pen colour | Current colour |
| ------------------- | --------------- | ----------- | -------------- |
| daily-results       | today           | `#9FA8DA`   | `#9FA8DA` ✓    |
| weekly-focus        | date_range      | `#9FA8DA`   | `#9FA8DA` ✓    |
| relations           | diversity_3     | `#9FA8DA`   | `#9FA8DA` ✓    |
| monthly-progress    | calendar_month  | `#42A5F5`   | `#9FA8DA` ✗    |
| outstanding-todos   | checklist       | `#FFA726`   | `#9FA8DA` ✗    |
| consistency-trend   | trending_up     | `#42A5F5`   | `#9FA8DA` ✗    |

Tile-shell SCSS hard-codes:

```scss
.tile-shell__icon {
  …
  color: var(--cui-primary, #9FA8DA);
}
```

Three tiles render their icons in the wrong theme colour.

The fix follows the same shape as bug-041 (per-tile body gap):
introduce a CSS custom property that tiles can opt into. Tile-shell
falls back to `--cui-primary` for the three already-correct tiles.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`

## Reproduction

1. Render monthly-progress, outstanding-todos, or consistency-trend.
2. Inspect the header icon — it renders in `#9FA8DA` (indigo).
3. Compare to the .pen — the icons match the tile's theme colour
   (info blue, warning amber, or info blue respectively).

## Expected

Tile-shell:

```scss
.tile-shell__icon {
  …
  color: var(--cui-tile-icon-color, var(--cui-primary, #9FA8DA));
}
```

Per-tile opt-ins (`:host { --cui-tile-icon-color: …; }`):
- monthly-progress, consistency-trend: `var(--cui-info, #42A5F5)`
- outstanding-todos: `var(--cui-warning, #FFA726)`

Daily-results, weekly-focus, relations keep the default (no
override needed).

## Verification

- Unit:
  - `tile-shell.component.spec.ts` asserts `.tile-shell__icon`
    declares a `color` consuming `var(--cui-tile-icon-color`.
  - monthly-progress, outstanding-todos, consistency-trend specs
    each assert their `:host` declares `--cui-tile-icon-color`.
- Visual: each themed tile's header icon matches its tile theme.
