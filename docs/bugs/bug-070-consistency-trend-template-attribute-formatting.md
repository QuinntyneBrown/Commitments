---
id: bug-070
title: Consistency-trend template uses long single-line attribute syntax; other 5 tiles use multi-line for readability
status: Fixed
---

# Bug 070 — Consistency-trend template attribute formatting

**Status**: Fixed

## Fix

Three over-long lines reformatted to multi-line attribute style:
- `<commitments-tile-shell>` open tag (was ~150 chars)
- `<cui-status-pill>` open tag (was ~115 chars)
- `<canvas #plot …>` (was 112 chars)

Brings consistency-trend's template into line with the multi-line
convention the other five plugin tiles already use.

Coverage:
- New spec asserts every line in the template ≤110 chars; will
  fail if a future contributor reverts to single-line syntax.
- All 22 affected suites pass (160/160 — was 159/159 before).

## Description

`consistency-trend-tile.component.html` opens with two long
single-line elements:

```html
<commitments-tile-shell title="Consistency Trend" eyebrow="7-day rolling average · last 14 days" icon="trending_up" variant="chart" data-testid="consistency-trend-tile">
  <cui-status-pill tile-status [variant]="pillVariant()" [pulse]="pillVariant() === 'chart'" [label]="statusLabel()">
  </cui-status-pill>
```

Each line is ~120-150 characters. The other five plugin tiles
(daily-results, weekly-focus, monthly-progress, outstanding-todos,
relations) use the multi-line attribute style:

```html
<commitments-tile-shell
  title="Daily Results"
  eyebrow="Today"
  icon="today"
  variant="metric"
  data-testid="daily-results-tile"
>
  <cui-status-pill
    tile-status
    [variant]="pillVariant()"
    [pulse]="pillVariant() === 'success'"
    [label]="statusLabel()"
  ></cui-status-pill>
```

Cross-tile style is uniform with multi-line; consistency-trend is
the lone outlier. Reformatting brings it into line with the
junior-readable convention.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

## Reproduction

```bash
awk 'length > 110 { print NR": "length" chars" }' \
  frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html
```

Two long lines (the tile-shell open tag and the status-pill).

## Expected

Both elements reformatted to multi-line attributes, matching the
other 5 tiles. No DOM or behaviour change — purely formatting.

## Verification

- Unit: source-level template spec asserts no line in the file
  exceeds 110 characters.
- All affected suites continue to pass — formatting only.
