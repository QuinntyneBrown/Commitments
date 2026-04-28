---
id: bug-035
title: Outstanding Todos — LIVE pill renders accent-pink, but the .pen otChip is warning-amber (#FFA726)
status: Fixed
---

# Bug 035 — Outstanding Todos pill colour wrong for warning-themed tile

**Status**: Fixed

## Fix

- `StatusPillVariant` gains a `'warning'` member.
- New `.status-pill--warning` rule uses `var(--cui-warning)` for the
  text/dot colour and a 14% tint for the background.
- `OutstandingTodosTileComponent` adds a `pillVariant` computed
  signal returning `'warning'` for live mode and `'review'` for
  review.
- Template `[variant]` and `[pulse]` read the computed; statusLabel
  still emits `LIVE`/`REVIEW`.

Mirrors the chart-variant (bug-031) and success-variant (bug-033)
patterns. Three near-identical themed rules now exist; SASS mixin
extraction is intentionally deferred to keep junior readability
(literal colour/tint relationships) and avoid retrofitting the
three matching source-level specs.

Coverage:
- `status-pill.component.spec.ts` asserts `.status-pill--warning`
  references `var(--cui-warning)`.
- `outstanding-todos-tile.component.spec.ts` asserts the TS source
  contains `pillVariant` returning `'warning'`.
- All 20 affected suites pass (95/95 — was 93/93 before).

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen` (frame `otChip`)
renders the header status indicator in warning amber:

- pill background `#FFA72622`
- text `4 OPEN` `#FFA726`

The whole tile is themed around `--cui-warning` — the icon, headline
count, and chip are all `#FFA726`. The implementation, however, uses
the shared `cui-status-pill` with `[variant]="controller.mode()"`,
which maps live mode to `--cui-accent` (pink). Result: a pink pulsing
`LIVE` pill in an otherwise amber card.

This is the same pattern of fix already applied for chart-themed
tiles (bug-031) and success-themed tiles (bug-033). The label
remains `LIVE`/`REVIEW` per project convention; only the colour
moves to match the tile theme.

## Affected files

- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.ts`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html`

## Reproduction

1. Render the Outstanding Todos tile in live mode.
2. Observe a pink pulsing `LIVE` pill in the header right slot.
3. Compare to the .pen — the chip is amber (`#FFA726`).

## Expected

- New `warning` variant on `StatusPillComponent` mapped to
  `var(--cui-warning)`.
- `OutstandingTodosTileComponent` exposes a `pillVariant` computed
  signal that maps live → `'warning'` and review → `'review'`.
- Pulse continues to fire in live snapshots.
- `pillLabel()` (via the existing `statusLabel`) keeps emitting
  `LIVE`/`REVIEW`.

## Verification

- Unit:
  - `status-pill.component.scss` declares a `.status-pill--warning`
    rule that references `var(--cui-warning)`.
  - `outstanding-todos-tile.component.ts` `pillVariant` returns
    `'warning'` somewhere in its body.
- Visual: the `LIVE` pill renders amber inside the outstanding-todos
  tile.
