---
id: bug-054
title: Delta-badge value/caption use different weight (700/500) and colour (accent/secondary); design renders the whole text uniformly Inter 11/500 in the accent colour
status: Fixed
---

# Bug 054 — Delta-badge typography splits value from caption; design is uniform

**Status**: Fixed

## Fix

`delta-badge.component.scss`:
- `.delta-badge` font-size 12px → 11px, font-weight 600 → 500.
- `.delta-badge__value` rule removed entirely.
- `.delta-badge__caption` rule removed entirely.

Both spans now inherit font-size / font-weight / colour
uniformly from the parent — matching the .pen ctDelta single
Inter 11/500 styled line.

Other consumers in `commitments-ui` (`line-chart-tile`,
`real-time-metric-tile`, `review-tile`) are demo components not
referenced by the live app, so the visual change is safely
scoped to the consistency-trend tile's actual rendering.

The consistency-trend wiring concern (passing both `[delta]` and
the API's full `caption` produces a duplicated number in the
displayed text) remains a separate bug to be filed when observed
in the browser.

Coverage:
- New spec asserts `.delta-badge` declares `font-size: 11px` and
  `font-weight: 500`, and that `__value` does not bump weight to
  ≥600 nor `__caption` overrides colour to `--cui-text-secondary`.
- All 20 affected suites pass (130/130 — was 129/129 before).

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `m5UjG` =
`ctDelta` text) declares the whole label as a single styled text:

- `font-family: Inter`
- `font-size: 11`
- `font-weight: 500`
- `fill: #66BB6A` (the same accent as the tone)

The implementation in `delta-badge.component.scss` currently splits
the label visually:

```scss
.delta-badge { font-size: 12px; font-weight: 600; … }
.delta-badge__value   { font-weight: 700; letter-spacing: 0.2px; }
.delta-badge__caption { color: var(--cui-text-secondary); font-size: 11px; font-weight: 500; }
```

Result: the `formatted()` value reads bold & accent-coloured, while
the caption reads regular & gray. The .pen has no such split.

The fix is to:
1. Drop the value rule's `font-weight: 700` and `letter-spacing`.
2. Drop the caption rule's secondary-colour override (and its now-
   redundant `font-size: 11px` / `font-weight: 500` since the
   parent supplies them).
3. Bump the parent `.delta-badge` to `font-size: 11px;
   font-weight: 500;` to match the .pen.

Both spans then inherit uniformly from `.delta-badge`.

The other consumers in `commitments-ui` (`line-chart-tile`,
`real-time-metric-tile`, `review-tile`) are demo components not
referenced from the live app, so the visual change is safely
scoped — the dashboard plugin's consistency-trend tile is the
only live consumer.

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the delta-badge — the value span renders bold, the
   caption span renders gray.
3. Compare to the .pen ctDelta — single uniform Inter 11/500 in
   the accent colour for the entire label.

## Expected

```scss
.delta-badge {
  …
  font-size: 11px;
  font-weight: 500;
}
/* Drop .delta-badge__value rule entirely */
/* Drop .delta-badge__caption rule entirely */
```

## Verification

- Unit:
  - `.delta-badge` declares `font-size: 11px` and `font-weight: 500`.
  - `.delta-badge__value` rule is absent (or empty).
  - `.delta-badge__caption` rule does not declare a colour
    override.
- Visual: the consistency-trend delta-badge reads as one
  continuous green line, no weight or colour breaks.
