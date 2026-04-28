---
id: bug-061
title: Consistency-trend uses `pillVariant()` and `pillLabel()` methods; other six tiles use `computed` signals
status: Fixed
---

# Bug 061 — Consistency-trend pill members are methods, others are computed signals

**Status**: Fixed

## Fix

`consistency-trend-tile.component.ts`:
- New `mode = computed(() => _tileContext?.mode?.() ?? 'live')`
- `pillVariant` now `computed<'chart' | 'review'>(...)` derived
  from `mode`.
- `pillLabel()` method renamed to `statusLabel` and converted
  to `computed(() => mode().toUpperCase())`.

Template binding `[label]="pillLabel()"` → `[label]="statusLabel()"`.

`computed` added to the existing `@angular/core` import.

Bug-031's earlier regex spec was loosened to accept either method
or computed form (mirrors the fix applied for bug-033 on the
daily-results spec).

Coverage:
- New TS-source spec asserts `mode`, `pillVariant`, and
  `statusLabel` are declared as `computed(...)` and the old
  `pillLabel(): string` method is gone.
- New template spec asserts `[label]` binds to `statusLabel()`.
- All 22 affected suites pass (147/147 — was 145/145 before).

Cross-tile convention: every dashboard-plugin tile now uses
computed signals for mode-derived state (label, variant, etc.).

## Description

`consistency-trend-tile.component.ts` declares its pill API as
plain methods:

```ts
pillVariant(): 'chart' | 'review' {
  return this._tileContext?.mode?.() === 'review' ? 'review' : 'chart';
}

pillLabel(): string {
  return (this._tileContext?.mode?.() ?? 'live').toUpperCase();
}
```

The other six dashboard-plugin tiles (daily-results, weekly-focus,
monthly-progress, outstanding-todos, relations, goal-metrics) use
`computed` signals:

```ts
protected readonly statusLabel = computed(() => this.controller.mode().toUpperCase());
protected readonly pillVariant = computed<…>(() => …);
```

Methods recompute on every change-detection pass; signals memoize
until their dependencies change. More importantly: a junior dev
reading the other five tiles sees a consistent idiom; the
seventh tile breaks the pattern.

The conversion involves:

1. Add a `mode` computed signal that reads `_tileContext?.mode?.()`
   once.
2. Convert `pillVariant` and `pillLabel` to `computed` signals
   that derive from `mode`.
3. Rename `pillLabel` → `statusLabel` to match the cross-tile
   property name.
4. Update the template binding from `pillLabel()` to
   `statusLabel()`.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

## Reproduction

```bash
grep -nE 'pillVariant|pillLabel|statusLabel' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
# Shows method declarations, not computed signals.
```

## Expected

```ts
protected readonly mode = computed(() => this._tileContext?.mode?.() ?? 'live');
protected readonly pillVariant = computed<'chart' | 'review'>(() =>
  this.mode() === 'review' ? 'review' : 'chart'
);
protected readonly statusLabel = computed(() => this.mode().toUpperCase());
```

Template:

```html
<cui-status-pill tile-status [variant]="pillVariant()" [pulse]="pillVariant() === 'chart'" [label]="statusLabel()">
```

## Verification

- Unit: source-level TS check — `consistency-trend-tile.component.ts`
  declares `pillVariant`, `mode`, and `statusLabel` as `computed`,
  not as methods.
- Template-source check: pill `[label]` binds to `statusLabel()`,
  not `pillLabel()`.
- All 22 affected suites continue to pass (no behaviour change).
