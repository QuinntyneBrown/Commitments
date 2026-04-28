---
id: bug-133
title: consistency-trend _fallbackMode / _fallbackDate use signal().asReadonly() for constant values; computed() is more idiomatic
status: Fixed
---

# Bug 133 — fallback signals: signal().asReadonly() → computed()

**Status**: Fixed

## Fix

Replaced both fallback declarations with `computed()`:

```ts
private readonly _fallbackMode = computed<'live' | 'review'>(() => 'live');
private readonly _fallbackDate = computed<string | null>(() => null);
```

`signal()` is for mutable state; `computed()` is for
derived/constant read-only signals. The now-unused `signal`
import was dropped. 329/329 workspace tests green.

## Description

The consistency-trend tile component declares two fallback
signals for the standalone (no-tile-context) case:

```ts
private readonly _fallbackMode = signal<'live' | 'review'>('live').asReadonly();
private readonly _fallbackDate = signal<string | null>(null).asReadonly();
```

`signal('live').asReadonly()` creates a writable signal with
value `'live'` and immediately casts it read-only. For a
**constant** read-only signal, `computed(() => 'live')` is the
idiomatic shape — `signal()` is for mutable state, `computed()`
is for derived/constant values.

Same observable behavior; cleaner intent.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n 'signal.*asReadonly' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Returns two matches.

## Expected

```ts
private readonly _fallbackMode = computed<'live' | 'review'>(() => 'live');
private readonly _fallbackDate = computed<string | null>(() => null);
```

`signal` import is dropped if no other usage; `computed` already
imported.

## Verification

- Unit (TS source): assert `signal(...)\.asReadonly()` is gone
  and the fallbacks use `computed(`.
- All existing consistency-trend specs continue to pass.
