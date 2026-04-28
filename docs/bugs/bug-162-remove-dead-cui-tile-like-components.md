---
id: bug-162
title: Remove 5 dead UI tile-like components from @commitments/ui
status: Fixed
---

# Bug 162 — Drop 5 dead tile-like UI components

**Status**: Fixed

## Fix

15 component files deleted (5 dirs × .ts/.html/.scss). 5 export
lines removed from `public-api.ts`. 536 lines net. 364/364
workspace tests green. Twelve more dead UI components remain
for follow-up bugs.

## Description

`@commitments/ui` exports 17 components, but cross-repo grep
shows only 7 are actually consumed by the workspace. This bug
removes the first batch — five tile-shaped components that
were scaffolded but never used:

| Component | Selector | Files |
|-----------|----------|-------|
| `CuiCardComponent` | `cui-card` | 3 |
| `ReviewTileComponent` | `cui-review-tile` | 3 |
| `TimelineScrubberComponent` | `cui-timeline-scrubber` | 3 |
| `LineChartTileComponent` | `cui-line-chart-tile` | 3 |
| `RealTimeMetricTileComponent` | `cui-real-time-metric-tile` | 3 |

Each is referenced only by its own definition files and the
`public-api.ts` barrel export. None has a spec file. Active
tile rendering is handled by `TileShellComponent` plus
per-tile components in `commitments-dashboard-plugin` — these
five are vestigial.

The remaining 12 dead components (button, chip, dialog-shell,
toolbar, sidenav, sidenav-item, app-shell, dashboard-tile,
radio-group, snackbar, table-row, text-input) will be
addressed in follow-up bugs to keep this iteration focused.

## Affected files (15 + 5 export lines)

For each component, three files in
`frontend/projects/commitments-ui/src/lib/<dir>/`:
- `<dir>.component.ts`
- `<dir>.component.html`
- `<dir>.component.scss`

Plus 5 `export *` lines removed from
`frontend/projects/commitments-ui/src/public-api.ts`.

## Reproduction

```bash
grep -rn 'CuiCardComponent\|ReviewTileComponent\|TimelineScrubberComponent\|LineChartTileComponent\|RealTimeMetricTileComponent' frontend/projects --include='*.ts' --include='*.html'
```

Returns matches only inside the components' own files.

## Expected

- 15 component files removed.
- 5 lines removed from `public-api.ts`.
- Regression-guard spec asserts each directory's
  `.component.ts` file no longer exists.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
