---
id: bug-155
title: DashboardLayoutStore.liveLayout / reviewLayout signals are unused — remove
status: Open
---

# Bug 155 — Drop unused `liveLayout` / `reviewLayout` readonly signals

## Description

`DashboardLayoutStore` exposes three signal-shaped accessors:

```ts
readonly liveLayout: Signal<DashboardItem[]> = this.liveItems.asReadonly();
readonly reviewLayout: Signal<DashboardItem[]> = this.reviewItems.asReadonly();
readonly items: Signal<DashboardItem[]> = computed(() =>
  this.modeService.mode() === 'live' ? this.liveItems() : this.reviewItems()
);
```

`items` is the canonical mode-aware accessor used by the
dashboard grid component. `liveLayout` and `reviewLayout` are
only referenced by one spec (lines 103-109 of the store spec)
which exists purely to assert that those public signals exist.
No production code reads either layout directly — the dashboard
flips between modes via `items`, never via the per-mode
signals.

Same YAGNI shape as bug-154's `listTiles()` — public surface
that exists only because tests test it. Drop both signals and
the spec block that exercises them.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-layout.store.ts` (drop two signal declarations)
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-layout.store.spec.ts` (drop the test that exercises them)

## Reproduction

```bash
grep -rn 'liveLayout\b\|reviewLayout\b' frontend/projects --include='*.ts'
```

Returns matches only inside the store and its spec.

## Expected

- `liveLayout` and `reviewLayout` are no longer declared on
  the store.
- The test that asserts both signals exist is removed.
- Regression-guard spec asserts both names are absent from the
  store source.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
