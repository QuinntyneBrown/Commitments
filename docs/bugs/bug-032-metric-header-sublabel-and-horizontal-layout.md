---
id: bug-032
title: Metric Header lacks a "today" sublabel and stacks value above caption; design lays them side-by-side with two captions stacked on the right
status: Open
---

# Bug 032 — Metric Header sublabel + horizontal layout

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `ctMetric`) lays
the headline metric out horizontally:

- left: large value (`78%`) — Inter 48 / 700 / `--cui-info`
- right: a small column (`ctMcol`, vertical, gap 2):
  - **top**: `"today"` — Inter 11 / `#B0B0B0` (label-like)
  - **bottom**: `"Peak 95% / Low 50%"` — Inter 11 / `#666666` (data)

The implementation in `metric-header.component.html` only renders the
**value** and a single **caption**, stacked vertically:

```html
<span class="metric-header__value">{{ value() }}</span>
@if (caption()) {
  <span class="metric-header__caption">{{ caption() }}</span>
}
```

`metric-header.component.scss` declares `display: grid; gap: 4px`,
which produces a single-column stack. As a result the consistency-
trend tile renders:

```
78%
Peak 95% / Low 50%
```

— missing the "today" sublabel entirely and laying the caption
*beneath* the value rather than to its right.

## Affected files

- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.ts`
- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.html`
- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

Blast radius: only `ConsistencyTrendTileComponent` projects
`<cui-metric-header>`, so the layout change is bounded.

## Reproduction

1. Render the Consistency Trend tile in live mode.
2. Headline reads `78%` on its own line, then `Peak 95% / Low 50%`
   beneath it. There is no `today` text anywhere.
3. Compare to the .pen — `78%` is on the left and a two-line
   stack `today` / `Peak 95% / Low 50%` is to its right.

## Expected

- New `subCaption` input on `MetricHeaderComponent` (defaults to `''`).
- When `caption` and/or `subCaption` are present, they render as
  stacked lines in a column to the **right** of the value, baseline-
  aligned to the bottom of the value (mirrors the .pen `alignItems: end`).
- `subCaption` renders a step darker (`#666666`) than `caption`.
- ConsistencyTrendTileComponent passes `caption="today"` and
  `[subCaption]="'Peak X% / Low Y%'"`.

## Verification

- Unit:
  - `metric-header.component.ts` declares a `subCaption` input.
  - `metric-header.component.scss` `.metric-header` rule uses
    `display: flex` (not grid) with `align-items: flex-end`.
  - `consistency-trend-tile.component.html` passes `caption="today"`
    and a `[subCaption]=` binding.
- Visual: screenshot the tile vs the .pen.
