---
id: bug-169
title: consistency-trend `_tileContext?.mode?.()` has a redundant optional chain
status: Open
---

# Bug 169 — Drop the redundant `?.` before `()` on `_tileContext.mode`

## Description

`consistency-trend-tile.component.ts` line 156:

```ts
protected readonly mode = computed(() => this._tileContext?.mode?.() ?? 'live');
```

The first `?.` (after `this._tileContext`) is needed because
the context is `inject(TILE_CONTEXT, { optional: true })` and
can be `undefined`. The second `?.` (after `mode`) is
unnecessary: once `_tileContext` is non-null, the
`TileContext` interface guarantees `mode: Signal<DashboardMode>`
is defined.

```ts
protected readonly mode = computed(() => this._tileContext?.mode() ?? 'live');
```

…has identical runtime behavior, one fewer character of noise,
and matches the in-effect pattern higher in the same file
(line 74-75) which already uses single `?.`:

```ts
this._tileContext?.mode();
this._tileContext?.selectedReviewDate();
```

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`

## Reproduction

```bash
grep -n '_tileContext?\.mode?\.()' frontend/projects/commitments-dashboard-plugin
```

Returns line 156.

## Expected

```ts
protected readonly mode = computed(() => this._tileContext?.mode() ?? 'live');
```

Regression-guard spec asserts the redundant `?.()` form is
absent.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
