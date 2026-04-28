---
id: bug-148
title: DashboardCardComponent (base) and PosterDashboardCardComponent are orphaned after bug-147 — delete
status: Open
---

# Bug 148 — Remove orphaned dashboard-card base + Poster intermediate

## Description

After bug-147 removed the 5 cardId-based leaf components, the
two ancestors in their inheritance chain are now genuinely
dead:

- `DashboardCardComponent` — abstract-style base class. Was
  extended by 5 leaves + 1 intermediate. Zero remaining
  consumers.
- `PosterDashboardCardComponent` — extended `DashboardCardComponent`,
  was itself extended only by `ToDoDashboardCardComponent` (now
  removed). Zero remaining consumers.

Cross-repo grep confirms each component is referenced only by
its own files. Same YAGNI argument as bugs 144-147: drop them.

## Affected files (6 files across 2 directories)

- `frontend/projects/commitments-app/src/app/components/dashboard-card/` (3 files)
- `frontend/projects/commitments-app/src/app/components/poster-dashboard-card/` (3 files)

## Reproduction

```bash
grep -rn 'DashboardCardComponent\b\|PosterDashboardCardComponent' frontend/projects --include='*.ts' --include='*.html'
```

Returns matches only inside each component's own definition.

## Expected

- The 6 files are removed.
- A regression-guard spec asserts both directories' `.component.ts`
  files no longer exist.

## Verification

- New regression spec confirms each component directory has
  been removed.
- All existing workspace tests continue to pass.
