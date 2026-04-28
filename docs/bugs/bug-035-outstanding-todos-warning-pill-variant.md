---
id: bug-035
title: Outstanding Todos — LIVE pill renders accent-pink, but the .pen otChip is warning-amber (#FFA726)
status: Open
---

# Bug 035 — Outstanding Todos pill colour wrong for warning-themed tile

**Status**: Open

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
