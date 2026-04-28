---
id: bug-182
title: monthly-progress .bars lacks 8px top padding from design
status: Fixed
---

# Bug 182 — monthly-progress `.bars` should have 8px top padding

**Status**: Fixed

## Fix

```scss
- padding: 0 4px;
+ padding: 8px 4px 0;
```

A 100% bar now leaves 8px above per design. 382/382
workspace tests green.

## Description

`monthly-progress-tile.component.scss` declares the bars
container with horizontal-only padding:

```scss
.bars {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  align-items: end;
  gap: 12px;
  height: 100%;
  min-height: 0;
  padding: 0 4px;
}
```

The Pencil design's `mpBars` frame has `padding: [8, 4, 0, 4]`
— **8px top**, 4px sides, 0 bottom. The 8px top is intentional
breathing room above the tallest bar; without it, a 100% bar
would touch the very top edge of the body, breaking the visual
rhythm vs the rest of the design system.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`

## Reproduction

Pull the design via Pencil — `mpBars` shows 8px above the
tallest bar. The implementation has zero top padding.

## Expected

```scss
.bars {
  ...
  padding: 8px 4px 0;
}
```

## Verification

- New regression spec asserts `.bars` `padding` includes
  the 8px top value.
- All other monthly-progress specs continue to pass.
