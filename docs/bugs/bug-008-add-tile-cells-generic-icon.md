# Bug 008 — Add Tile catalog cells all show the same generic icon

**Status**: Fixed

## Description

Every tile in the Add Tile dialog shows the same `dashboard` Material Symbol icon because no tile defines an `icon` property in its `tileMetadata`. The dialog template falls back to `tile.icon || 'dashboard'`.

The design specifies tile-specific icons:
- Daily Results → `today`
- Weekly Focus → `date_range`
- Monthly Progress → `calendar_month`
- Outstanding To‑Dos → `checklist`
- Relations → `diversity_3`
- Consistency Trend → `trending_up`
- Live Goal Metrics → `track_changes`
- Review Goal History → `history`

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-app/src/app/components/live-goal-metrics-tile/live-goal-metrics-tile.component.ts`
- `frontend/projects/commitments-app/src/app/components/review-goal-history-tile/review-goal-history-tile.component.ts`

## Fix

Add `icon` property to each tile's `tileMetadata` matching the design.
