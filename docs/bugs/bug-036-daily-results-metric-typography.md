---
id: bug-036
title: Daily Results — metric value renders at 42px / 800 and label at 13px; design specifies 48px / 700 and 12px
status: Fixed
---

# Bug 036 — Daily Results metric typography

**Status**: Fixed

## Fix

`daily-results-tile.component.scss`:
- `.metric__value` font-size 42px → 48px, font-weight 800 → 700
- `.metric__label` font-size 13px → 12px

Colours already matched via the existing tokens
(`var(--cui-success)` and `var(--cui-text-secondary)`).

Coverage:
- Two new specs in `daily-results-tile.component.spec.ts` assert
  the value's 48px / 700 and the label's 12px.
- All 20 affected suites pass (97/97 — was 95/95 before).

## Description

`docs/tiles/daily-results-tile/ui-design.pen` (frame `sXIl5` →
`drBody`) styles the headline and supporting line as:

| Element                   | Family | Size | Weight | Colour    |
| ------------------------- | ------ | ---- | ------ | --------- |
| `7 / 9` (`uHh8p`)         | Inter  | 48   | 700    | `#66BB6A` |
| `commitments completed`   | Inter  | 12   | 400    | `#B0B0B0` |

The implementation in `daily-results-tile.component.scss` styles
them as:

```scss
.metric__value { font-size: 42px; font-weight: 800; }
.metric__label { font-size: 13px; }
```

Three deviations: value is 6px smaller, value weight is one stop
heavier, label is 1px larger. Visually the headline reads chunkier
and tighter than the .pen.

The colour tokens (`var(--cui-success)` / `var(--cui-text-secondary)`)
already match the .pen `#66BB6A` / `#B0B0B0` after token resolution,
so no colour change is needed.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`

## Reproduction

1. Render the Daily Results tile.
2. Inspect `.metric__value` — `font-size: 42px; font-weight: 800;`.
3. Inspect `.metric__label` — `font-size: 13px;`.
4. Compare to the .pen — value is 48px / 700, label is 12px / 400.

## Expected

```scss
.metric__value { font-size: 48px; font-weight: 700; }
.metric__label { font-size: 12px; }
```

## Verification

- Unit: source-level SCSS spec — `.metric__value` declares
  `font-size: 48px; font-weight: 700;` and `.metric__label` declares
  `font-size: 12px;`.
- Visual: screenshot the rendered tile vs the .pen.
