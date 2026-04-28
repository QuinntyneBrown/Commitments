---
id: bug-049
title: Tile-shell icon hard-codes 20×20; consistency-trend design specifies 22×22
status: Fixed
---

# Bug 049 — Tile-shell icon size is fixed at 20px instead of per-tile-design

**Status**: Fixed

## Fix

`tile-shell.component.scss`:
- `.tile-shell__icon` `font-size`, `line-height`, `width`, `height`
  consume `var(--cui-tile-icon-size, 20px)`.

`consistency-trend-tile.component.scss`:
- `:host` declares `--cui-tile-icon-size: 22px`.

Other five tiles inherit the 20px default (no override needed).

Three related per-tile customization slots now exist on the
tile-shell:
- `--cui-tile-body-gap` (bug-041)
- `--cui-tile-icon-color` (bug-048)
- `--cui-tile-icon-size` (bug-049)

Coverage:
- `tile-shell.component.spec.ts` asserts `.tile-shell__icon`
  consumes `var(--cui-tile-icon-size)` for font-size, width, and
  height.
- `consistency-trend-tile.component.spec.ts` asserts `:host`
  declares `--cui-tile-icon-size: 22px`.
- All 20 affected suites pass (121/121 — was 119/119 before).

## Description

Tile-shell's `.tile-shell__icon` uses fixed sizing:

```scss
.tile-shell__icon {
  font-size: 20px;
  line-height: 20px;
  width: 20px;
  height: 20px;
  …
}
```

Five of the six tile designs do specify 20×20 (daily-results,
weekly-focus, monthly-progress, outstanding-todos, relations).

The consistency-trend design (`docs/tiles/consistency-trend/ui-design.pen`,
node `o0wNsX`) specifies 22×22 — the chart tile gets a slightly
larger header icon to balance its larger overall size (720×420 vs
~320×200 for the others).

A 2px difference, single outlier. The radically-simple fix mirrors
bug-041 (per-tile body gap) and bug-048 (per-tile icon colour) —
introduce a `--cui-tile-icon-size` custom property and have
consistency-trend opt in to 22.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the header icon — `font-size`, `width`, `height` all
   `20px`.
3. Compare to the .pen — icon is 22×22.

## Expected

```scss
.tile-shell__icon {
  font-size: var(--cui-tile-icon-size, 20px);
  line-height: var(--cui-tile-icon-size, 20px);
  width: var(--cui-tile-icon-size, 20px);
  height: var(--cui-tile-icon-size, 20px);
  …
}
```

Consistency-trend host:

```scss
:host {
  …
  --cui-tile-icon-size: 22px;
}
```

## Verification

- Unit:
  - `tile-shell.component.spec.ts` asserts `.tile-shell__icon`
    declares `font-size`, `width`, `height` consuming
    `var(--cui-tile-icon-size`.
  - `consistency-trend-tile.component.spec.ts` asserts `:host`
    declares `--cui-tile-icon-size: 22px`.
- Visual: consistency-trend's header icon renders 22×22, the other
  five tiles stay at 20×20.
