---
id: bug-101
title: consistency-trend-tile.component.scss var() call omits the design-system fallback hex
status: Open
---

# Bug 101 — consistency-trend var() fallback hex

**Status**: Open

## Description

Same shape as bug-096/097/098/099/100 — the final tile in the
series. `consistency-trend-tile.component.scss` has one
remaining `var(--cui-*)` reference without the cui-ui-style
hex fallback:

```scss
:host {
  …
  --cui-tile-icon-color: var(--cui-info);
  …
}
```

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss
```

Returns one match.

## Expected

`var(--cui-info, #42A5F5)` for `--cui-tile-icon-color`.

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing consistency-trend specs continue to pass.
