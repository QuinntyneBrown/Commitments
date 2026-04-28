---
id: bug-056
title: Consistency Trend tile-shell is missing the `data-testid` attribute that the other five tiles carry
status: Fixed
---

# Bug 056 — Consistency Trend tile-shell missing data-testid

**Status**: Fixed

## Fix

`consistency-trend-tile.component.html` now carries
`data-testid="consistency-trend-tile"` on the
`<commitments-tile-shell>` element, matching the convention the
other five plugin tiles already follow. Tile chrome addressable
from Playwright POMs that use uniform tile-id selectors.

Coverage:
- New template-source spec asserts the attribute is present on
  the tile-shell element.
- All 20 affected suites pass (136/136 — was 135/135 before).

Auto-rendering the attribute from `tileMetadata.tileId` via a
shell input is deferred — the migration cost outweighs the
cleanup with 5 tiles already manually carrying it.

## Description

The five other plugin tiles each declare a tile-level `data-testid`
on their `<commitments-tile-shell>` element:

- `daily-results-tile`     → `data-testid="daily-results-tile"`
- `weekly-focus-tile`      → `data-testid="weekly-focus-tile"`
- `monthly-progress-tile`  → `data-testid="monthly-progress-tile"`
- `outstanding-todos-tile` → `data-testid="outstanding-todos-tile"`
- `relations-tile`         → `data-testid="relations-tile"`

The consistency-trend tile shell omits this attribute:

```html
<commitments-tile-shell title="Consistency Trend" eyebrow="…" icon="trending_up" variant="chart">
```

Cross-tile consistency: an e2e suite or Playwright POM that
addresses tiles by their tile-id can locate the other five but
not consistency-trend. The attribute belongs on the shell to make
the tile's outer chrome addressable, alongside the existing
`data-testid="consistency-trend-canvas"` on the inner canvas
element.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

## Reproduction

```bash
grep 'data-testid="consistency-trend-tile"' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/
# (no match)
```

## Expected

```html
<commitments-tile-shell
  title="Consistency Trend"
  eyebrow="7-day rolling average · last 14 days"
  icon="trending_up"
  variant="chart"
  data-testid="consistency-trend-tile">
```

## Verification

- Unit: source-level template check —
  `consistency-trend-tile.component.html` declares
  `data-testid="consistency-trend-tile"` on the tile-shell.
- Visual / e2e: the tile is addressable via
  `[data-testid="consistency-trend-tile"]`.
