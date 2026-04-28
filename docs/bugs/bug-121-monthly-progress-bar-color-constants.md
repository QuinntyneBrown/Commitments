---
id: bug-121
title: monthly-progress bar gradient uses magic hex codes #8bb8e8 / #2f6db3 with no descriptive name
status: Open
---

# Bug 121 — bar gradient color constants

**Status**: Open

## Description

bug-120 fixed the gradient direction but left the colours as
magic hex codes:

```scss
.bars span {
  background: linear-gradient(180deg, #8bb8e8, #2f6db3);
}
```

`#8bb8e8` is the lighter top stop and `#2f6db3` the darker
bottom stop, but the file has no name for either. Same shape
as bug-116 (today-glow magic numbers); extracting to SCSS
variables documents intent and avoids re-deriving the meaning.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.spec.ts`

## Reproduction

```bash
grep -n "linear-gradient" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss
```

Two anonymous hex codes inside the gradient.

## Expected

Two named SCSS variables at the top of the file documenting
each stop's role:

```scss
$bar-top: #8bb8e8;
$bar-bottom: #2f6db3;
.bars span {
  background: linear-gradient(180deg, $bar-top, $bar-bottom);
}
```

## Verification

- Unit (CSS source): assert the file declares `$bar-top:
  #8bb8e8` and `$bar-bottom: #2f6db3` and the `.bars span` rule
  references them. Refactor only — visual output unchanged.
- All existing monthly-progress specs continue to pass.
