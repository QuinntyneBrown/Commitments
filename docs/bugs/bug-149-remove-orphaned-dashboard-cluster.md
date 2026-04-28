---
id: bug-149
title: Remove orphaned dashboard cluster (4 services, 2 dialog components, 2 models)
status: Fixed
---

# Bug 149 — Remove final orphaned legacy dashboard cluster

**Status**: Fixed

## Fix

Deleted 12 files — 447 lines net. Note the dialog component
templates are named `*.html` / `*.scss` (without the
`.component` infix) — different filename pattern than the rest
of the workspace. Regression-guard spec asserts every file is
gone. 357/357 workspace tests green.

The four-bug arc (146→147→148→149) has now removed the
complete legacy cardId-based dashboard infrastructure: the
page, 5 leaf cards, 1 intermediate, the base class, 4
services, 2 dialog components, and 2 models. Total: ~1100
lines of dead code, 33 files.

## Description

The bug-146 → 147 → 148 deletion arc removed the legacy
`DashboardPageComponent` and its component descendants. What
remains is a cluster of 4 services, 2 dialog components, and 2
models that were tightly coupled to the legacy dashboard. Each
file in this cluster is referenced ONLY by other files in the
same cluster — no live consumer.

## Cluster contents (12 files)

Services (1 file each):
- `services/dashboard.service.ts` — zero references outside its own file
- `services/dashboard-card.service.ts` — referenced only by the two dialog components
- `services/add-dashboard-cards-dialog.service.ts` — references its dialog component
- `services/dashboard-card-configuration-dialog.service.ts` — references its dialog component

Dialog components (3 files each):
- `components/add-dashboard-cards-dialog/`
- `components/dashboard-card-configuration-dialog/`

Models (1 file each):
- `models/dashboard.ts` — zero references anywhere
- `models/dashboard-card.ts` — referenced only by the two dialog components

The cluster is mutually self-referential and collectively dead.
Removing all 12 files in one shot keeps the cascade contained.

## Reproduction

```bash
grep -rn 'DashboardService\b\|DashboardCardService\b\|AddDashboardCardsDialog\|DashboardCardConfigurationDialog\|from .*/models/dashboard' frontend/projects --include='*.ts'
```

Returns matches only inside the cluster files themselves.

## Expected

The 12 files are removed. A regression-guard spec asserts each
file is gone.

## Verification

- New regression spec confirms each cluster file no longer
  exists.
- All existing workspace tests continue to pass.
