---
id: bug-042
title: Tile-shell sets `box-shadow: none`, overriding the .pen tile drop shadow on every tile
status: Fixed
---

# Bug 042 — Tile-shell drops the .pen drop shadow

**Status**: Fixed

## Fix

`tile-shell.component.scss` swaps `box-shadow: none` for
`box-shadow: 0 4px 12px rgba(0, 0, 0, 0.6)`, matching the .pen tile
shadow effect (offset y 4, blur 12, `#00000099` = 60% black).

Coverage:
- New spec `renders the .pen drop shadow on every tile (bug-042)`
  asserts the rule contains a `box-shadow` with both `4px` and
  `12px` and is not `none`.
- All 20 affected suites pass (110/110 — was 109/109 before).

Token extraction (`--cui-shadow-tile`) deferred until a second
consumer adopts the same shadow.

## Description

Every tile design in `docs/tiles/` declares an outer drop shadow on
its top-level frame:

```json
{
  "effect": {
    "type": "shadow",
    "shadowType": "outer",
    "color": "#00000099",
    "offset": { "x": 0, "y": 4 },
    "blur": 12
  }
}
```

CSS equivalent: roughly `box-shadow: 0 4px 12px rgba(0, 0, 0, 0.6)`.

`tile-shell.component.scss` instead sets:

```scss
.tile-shell {
  …
  box-shadow: none;   // explicitly removes Material Card's default
}
```

Tiles render flat across the dashboard. Material Card's default
elevation shadow is what tile-shell was probably trying to suppress
— but the design wants its own drop shadow, not none.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

1. Render any tile.
2. Inspect `.tile-shell` — `box-shadow: none`.
3. Compare to the .pen — every tile floats over the dashboard with
   a soft drop shadow.

## Expected

```scss
.tile-shell {
  …
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.6);
}
```

The 60% black `#00000099` translates to `rgba(0, 0, 0, 0.6)`.

## Verification

- Unit: source-level SCSS spec — `.tile-shell` declares a
  `box-shadow` whose value is **not** `none` and contains the .pen
  offset/blur/colour values.
- Visual: every tile now floats with a soft drop shadow matching
  the .pen.
