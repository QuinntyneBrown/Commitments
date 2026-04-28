---
id: bug-086
title: delta-badge uses align-items baseline, the design's ctDelta uses alignItems center
status: Fixed
---

# Bug 086 — delta-badge baseline vs center alignment

**Status**: Fixed

## Fix

Changed `.delta-badge { align-items }` from `baseline` to
`center`, matching the design's `ctDelta.alignItems`. The 12px
Material arrow icon now sits visually centered against the 11px
Inter text — instead of dropping below center where the
baseline alignment placed it (Material's icon-font baseline is
hand-tuned below visual center).

`consistency-trend-tile` is the only consumer of delta-badge.
11/11 delta-badge specs green.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s `ctDelta` frame
specifies `alignItems: "center"` for its row of [icon, text]:

```
{ alignItems: "center", children: [{icon 12x12}, {text}], ... }
```

`delta-badge.component.scss:7` instead has:

```scss
.delta-badge {
  display: inline-flex;
  align-items: baseline;
  …
}
```

`baseline` aligns the elements to the text baseline. With a
12px Material Symbols icon next to 11px Inter text — and the
Material font's hand-tuned baseline being below visual center —
the icon ends up sitting slightly below the visual center of the
text, producing a clearly misaligned chip.

`center` matches the design and is the conventional alignment
for a small-icon-plus-label inline pattern.

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss`
- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.spec.ts`

## Reproduction

```bash
grep -n "align-items" frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss
```

Returns one match: `align-items: baseline;`.

## Expected

`.delta-badge { align-items: center }`, matching the design's
`ctDelta.alignItems`.

## Verification

- Unit (CSS source): assert `.delta-badge` block uses
  `align-items: center` and not `baseline`.
- All existing delta-badge specs continue to pass.
