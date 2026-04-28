---
id: bug-124
title: consistency-trend uses legacy @ViewChild decorator instead of signal-based viewChild()
status: Fixed
---

# Bug 124 — @ViewChild → viewChild() migration

**Status**: Fixed

## Fix

Replaced the decorator declaration with `viewChild`:

```ts
readonly plotRef = viewChild<ElementRef<HTMLCanvasElement>>('plot');
```

Two consumers updated to call the signal: the `effect()`
truthy check and `ngAfterViewInit` (uses an explicit local ref
guard before `attach`). Completes the modern signal alignment
started by bug-071. 321/321 workspace tests green.

## Description

bug-071 migrated `@Input()` to signal-based `input()` on
`ConsistencyTrendTileComponent`. The remaining legacy decorator
is `@ViewChild`:

```ts
@ViewChild('plot') plotRef!: ElementRef<HTMLCanvasElement>;
```

Angular 17.2+ exposes a signal-based `viewChild()` query that
returns a read-only signal. Migrating finishes the modern
signal alignment for the file (and parallels the bug-071 work).

Two consumers need updating:

```ts
// Before
if (this.plotRef) { … }
this._adapter.attach(this.plotRef, …);

// After
plotRef = viewChild<ElementRef<HTMLCanvasElement>>('plot');
…
const ref = this.plotRef();
if (ref) { … }
this._adapter.attach(ref, …);
```

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n '@ViewChild\|@Input' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

One `@ViewChild` line.

## Expected

`@ViewChild` is removed; `viewChild` is imported from
`@angular/core` and used to declare `plotRef`. Consumers call
the signal to read the ref.

## Verification

- Unit (TS source): assert `@ViewChild` is gone, `viewChild`
  is imported, and `plotRef = viewChild(…` is declared.
- All existing consistency-trend specs continue to pass.
