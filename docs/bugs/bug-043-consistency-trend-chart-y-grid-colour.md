---
id: bug-043
title: Consistency Trend chart Y-axis grid lines render `#3A3A3A` (warm gray); design uses muted white at ~7% alpha
status: Open
---

# Bug 043 — Consistency Trend chart grid colour mismatch

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `e5bRb` →
`ltPlot`) draws five horizontal grid lines on the chart background
in low-opacity white:

| Line | Y position | Colour      | Alpha (%) |
| ---- | ---------- | ----------- | --------- |
| g1   | 10         | `#FFFFFF11` | 6.7       |
| g2   | 50         | `#FFFFFF0D` | 5.1       |
| g3   | 90         | `#FFFFFF0D` | 5.1       |
| g4   | 130        | `#FFFFFF0D` | 5.1       |
| g5   | 170        | `#FFFFFF1A` | 10.2      |

Average ~6% alpha white. The visual is muted, almost invisible
gridlines that don't compete with the trend line.

The implementation in
`consistency-trend-tile.component.ts` configures the Chart.js Y-axis
grid as:

```ts
y: { min: 0, max: 100, ticks: { display: false }, grid: { color: '#3A3A3A', drawTicks: false } }
```

`#3A3A3A` is the warm-gray divider token. On the `#242424` tile
surface it reads as a slightly warmer, slightly more visible line
than the design's white-tinted equivalent. Switching to white at
~7% alpha brings the grid into the .pen palette (and drops the
hint of warmth that doesn't belong on the chart).

Per-line opacity (the design uses 5%/6.7%/10.2% across rows) is not
configurable in Chart.js without a custom plugin, so a flat ~7%
average is the radically-simple approach.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`

## Reproduction

1. Render the Consistency Trend tile with seeded data.
2. Inspect the chart — horizontal Y-grid lines appear in `#3A3A3A`.
3. Compare to the .pen — they're muted white.

## Expected

```ts
y: { …, grid: { color: 'rgba(255, 255, 255, 0.07)', drawTicks: false } }
```

## Verification

- Unit: source-level TS check on
  `consistency-trend-tile.component.ts` — the y-axis grid color
  declaration uses an `rgba(255, 255, 255, …)` value, not
  `#3A3A3A`.
- Visual: the chart's gridlines now read as muted white on the
  dark tile surface.
