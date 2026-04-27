# 075 — Consistency Trend defaultSize too short to show the chart at lg

## Status

OPEN — surfaced by the follow-up note on bug 074.

## Symptom

After 074 the chart is properly contained in its tile, but at the
default size `{ cols: 6, rows: 4 }` on lg-desktop (1280×800), the plot
ends up ~29px tall (status pill 30 + metric ~80 + delta ~30 +
padding ~24 leaves only ~29px for the chart). The chart is visible
but unreadable.

At xl-desktop the same size renders a beautiful 731×169 chart, so
this is a lg-specific space-budget issue.

## Fix

Bump `defaultSize` from `{ cols: 6, rows: 4 }` to `{ cols: 6, rows: 5 }`.
That gives the cell ~400×80 = 400px tall at lg, leaving the plot
~110px (≈4× more) — enough for the line to be readable.

Persisted layouts in `localStorage` keep their existing sizes; only
newly-added tiles get the new default.

## Resolution

- [ ] Visual screenshot pre-fix at lg.
- [ ] defaultSize updated.
- [ ] Visual screenshot post-fix shows readable chart at lg.
