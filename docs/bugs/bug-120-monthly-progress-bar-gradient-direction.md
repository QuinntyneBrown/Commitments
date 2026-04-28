---
id: bug-120
title: Monthly Progress bar gradient direction is reversed — design shows lighter at top, impl renders darker at top
status: Fixed
---

# Bug 120 — bar gradient direction

**Status**: Fixed

## Fix

Swapped the two stops in the bar's `linear-gradient(180deg, ...)`
from `#2f6db3, #8bb8e8` (darker → lighter, top to bottom) to
`#8bb8e8, #2f6db3` (lighter → darker, top to bottom). The bars
now render brighter at the top and deeper blue at the bottom,
matching the design screenshot.

317/317 workspace tests green.

## Description

`docs/tiles/monthly-progress-tile/ui-design.pen`'s screenshot
shows the four week bars with a gradient that's **brighter at
the top** (closer to `#8bb8e8`) and **deeper blue at the
bottom** (closer to `#2f6db3`).

`monthly-progress-tile.component.scss:20` writes:

```scss
.bars span {
  background: linear-gradient(180deg, #2f6db3, #8bb8e8);
}
```

CSS `linear-gradient(180deg, A, B)` flows downward: A at top,
B at bottom. So with the colours in this order — `#2f6db3`
(darker) first, `#8bb8e8` (lighter) second — the impl puts
**darker at the top** and lighter at the bottom. Opposite of
the design.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.spec.ts`

## Reproduction

```bash
grep -n "linear-gradient" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss
```

`linear-gradient(180deg, #2f6db3, #8bb8e8)` — darker first.

## Expected

`linear-gradient(180deg, #8bb8e8, #2f6db3)` — lighter at top,
darker at bottom. (Or the equivalent `linear-gradient(0deg,
#2f6db3, #8bb8e8)`.)

## Verification

- Unit (CSS source): assert the `.bars span` block contains a
  `linear-gradient` whose first colour is `#8bb8e8` (lighter)
  followed by `#2f6db3` (darker).
- All existing monthly-progress specs continue to pass.
