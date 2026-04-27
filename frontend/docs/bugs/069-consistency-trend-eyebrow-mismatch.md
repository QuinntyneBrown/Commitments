# 069 — Consistency Trend tile eyebrow doesn't match design `9IpBQ`

## Status

FIXED — eyebrow now reads "7-DAY ROLLING AVERAGE · LAST 14 DAYS"
(uppercased by tile-shell SCSS); visual screenshot confirms.

## Symptom

Running consistency-trend tile shows eyebrow **"Goal completion rate"**.
Design `Line-Chart-Tile (9IpBQ)` shows the more descriptive eyebrow
**"7-day rolling average · last 14 days"** — describes what the chart
itself plots, not just the metric.

## Fix

Update the eyebrow in `consistency-trend-tile.component.html` from
`eyebrow="Goal completion rate"` → `eyebrow="7-day rolling average · last 14 days"`.

## Resolution

- [x] Visual screenshot pre-fix shows "GOAL COMPLETION RATE".
- [x] Eyebrow updated to `"7-day rolling average · last 14 days"`.
- [x] Visual screenshot post-fix shows the new eyebrow.
