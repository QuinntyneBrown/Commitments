---
id: bug-147
title: 5 cardId-based dashboard-card components are orphaned after bug-146 — delete
status: Open
---

# Bug 147 — Remove orphaned legacy dashboard-card leaf components

## Description

Bug-146 deleted the legacy `DashboardPageComponent`, which was
the only consumer of these 5 cardId-based dashboard cards:

| Component | Selector |
|-----------|----------|
| DailyResultsDashboardCardComponent | `app-daily-results-dashboard-card` |
| WeeklyResultsDashboardCardComponent | `app-weekly-results-dashboard-card` |
| MonthlyResultsDashboardCardComponent | `app-monthly-results-dashboard-card` |
| ToDoDashboardCardComponent | `app-to-do-dashboard-card` |
| RelationsResultsDashboardCardComponent | `app-relations-results-dashboard-card` |

Cross-repo grep confirms each component is referenced ONLY by
its own three files (`.ts`, `.html`, `.scss`) — there are no
remaining imports, no template usage, no specs.

Same YAGNI argument as bug-144 / bug-145 / bug-146: zero
production callers means dead code. Drop them. The base
`DashboardCardComponent` and the intermediate
`PosterDashboardCardComponent` will be addressed in a follow-up
bug once this leaf cleanup makes their orphan status verifiable.

## Affected files (15 files across 5 directories)

- `frontend/projects/commitments-app/src/app/components/daily-results-dashboard-card/` (3 files)
- `frontend/projects/commitments-app/src/app/components/weekly-results-dashboard-card/` (3 files)
- `frontend/projects/commitments-app/src/app/components/monthly-results-dashboard-card/` (3 files)
- `frontend/projects/commitments-app/src/app/components/to-do-dashboard-card/` (3 files)
- `frontend/projects/commitments-app/src/app/components/relations-results-dashboard-card/` (3 files)

## Reproduction

```bash
grep -rn 'DailyResultsDashboardCardComponent\|WeeklyResultsDashboardCardComponent\|MonthlyResultsDashboardCardComponent\|ToDoDashboardCardComponent\|RelationsResultsDashboardCardComponent' frontend/projects --include='*.ts' --include='*.html'
```

Returns matches only inside each component's own definition.

## Expected

The 15 files are removed. A regression-guard spec asserts the
five legacy `app-…-dashboard-card` selectors are absent from the
codebase.

## Verification

- New regression spec confirms each dashboard-card directory
  has been removed.
- All existing workspace tests continue to pass.
