---
id: bug-150
title: AchievementService + Achievement model are orphaned after bug-147 — delete
status: Fixed
---

# Bug 150 — Remove orphaned `AchievementService` and `Achievement` model

**Status**: Fixed

## Fix

Deleted 2 files (44 lines net). Regression-guard spec asserts
neither file exists. 358/358 workspace tests green.

## Description

`DailyResultsDashboardCardComponent` (deleted in bug-147) was
the only consumer of `AchievementService`. After that removal,
the service and its DTO model are mutually orphaned:

- `AchievementService` — zero references outside its own file.
- `Achievement` model — referenced only by the service.

Same YAGNI argument as bugs 144-149.

## Affected files (2)

- `frontend/projects/commitments-app/src/app/services/achievement.service.ts`
- `frontend/projects/commitments-app/src/app/models/achievement.ts`

## Reproduction

```bash
grep -rn 'AchievementService\b\|from .*/models/achievement' frontend/projects --include='*.ts'
```

Returns matches only inside the two files themselves.

## Expected

Both files removed; regression-guard spec asserts neither
exists.

## Verification

- New regression spec confirms each file is gone.
- All existing workspace tests continue to pass.
