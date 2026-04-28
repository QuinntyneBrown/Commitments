---
id: bug-143
title: consistency-trend `.plot` uses literal margin-top instead of --cui-tile-body-gap token
status: Open
---

# Bug 143 — consistency-trend `.plot { margin-top: 14px }` should reference the body-gap token

## Description

The consistency-trend tile defines its body gap as a CSS
custom property:

```scss
:host {
  --cui-tile-body-gap: 14px;
  ...
}
```

`tile-shell.component.scss` uses that same token for the
header→body gap:

```scss
.tile-shell__body {
  ...
  padding-top: var(--cui-tile-body-gap, 10px);
}
```

But inside the tile, the chart canvas wrapper hardcodes the
literal value instead of referencing the token:

```scss
.plot {
  ...
  margin-top: 14px;
}
```

The two values happen to match today (both 14px), but they are
expressing the same semantic concept — "tile body row gap" —
and should be a single source of truth. If a future design
iteration changes the token, the plot margin silently drifts.

This is the same fix shape applied in bug-085
(daily-results `.progress`) and bug-095 — switch the literal to
`var(--cui-tile-body-gap, 14px)` so the token is the canonical
value and the literal is just the same fallback.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts` (extend regression guard)

## Reproduction

```bash
grep -n 'margin-top' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss
```

Returns one match: `margin-top: 14px;` (no token reference).

## Expected

```scss
.plot {
  ...
  margin-top: var(--cui-tile-body-gap, 14px);
}
```

## Verification

- Source-level spec asserts `.plot` `margin-top` references
  `--cui-tile-body-gap`.
- All existing consistency-trend specs continue to pass.
