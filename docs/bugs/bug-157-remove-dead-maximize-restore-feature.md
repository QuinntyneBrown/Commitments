---
id: bug-157
title: Maximize/restore feature is fully unused — drop from TileContext, DashboardItem, dashboard-grid
status: Open
---

# Bug 157 — Drop dead maximize/restore feature

## Description

`TileContext` declares three coupled members for tile
maximize/restore:

```ts
readonly isMaximized: Signal<boolean>;
maximize(): void;
restore(): void;
```

…and `DashboardItem` carries a `maximized: boolean` field.
Cross-repo grep confirms zero callers of `.maximize()` or
`.restore()` on a `TileContext`, and zero readers of
`isMaximized()` or `item.maximized`. The dashboard-grid
template has no maximize/restore button — the feature was
scaffolded but never wired to UI.

Same shape as bug-156's refresh-pipeline removal. Drop the
whole feature as a unit:

- `TileContext.isMaximized`, `maximize()`, `restore()` —
  remove from interface
- `dashboard-grid.component.ts` — drop the
  `maximizedSignal: Signal<boolean>` plumbing and the three
  stub callbacks
- `DashboardItem.maximized` — drop the model field
- `dashboard-layout.store.createItem()` — drop the
  `maximized: false` initializer
- `layout-persistence.service.spec.ts` — drop the field from
  fixtures

Existing user dashboards in localStorage may have
`maximized: false` entries. The persistence loader's
`isPersistedLayout` predicate validates only `schemaVersion`
and `items` array shape — extra fields on items are silently
tolerated. No schema migration required.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile.model.ts`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard.model.ts`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.ts`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-layout.store.ts`
- `frontend/projects/dashboard-framework/src/lib/dashboard/layout-persistence.service.spec.ts`

## Reproduction

```bash
grep -rn '\.maximize(\|\.restore(\|\.isMaximized\b' frontend/projects --include='*.ts'
```

Returns no matches outside the framework's own definitions.
The only `.restore()` hit anywhere is `ctx.restore()` on a
Chart.js canvas state in `consistency-trend-tile` (unrelated).

## Expected

- Interface members and field removed.
- Regression-guard spec asserts the symbols are absent from
  `tile.model.ts` and `dashboard.model.ts`.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
