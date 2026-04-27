# 069 — Consistency Trend tile eyebrow doesn't match design `9IpBQ`

## Status

OPEN — surfaced via design comparison.

## Symptom

Running consistency-trend tile shows eyebrow **"Goal completion rate"**.
Design `Line-Chart-Tile (9IpBQ)` shows the more descriptive eyebrow
**"7-day rolling average · last 14 days"** — describes what the chart
itself plots, not just the metric.

## Fix

Update the eyebrow in `consistency-trend-tile.component.html` from
`eyebrow="Goal completion rate"` → `eyebrow="7-day rolling average · last 14 days"`.

## Resolution

- [ ] Visual screenshot pre-fix.
- [ ] Eyebrow updated.
- [ ] Visual screenshot post-fix shows new eyebrow.
