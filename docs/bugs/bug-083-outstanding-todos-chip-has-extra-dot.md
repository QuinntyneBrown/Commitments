---
id: bug-083
title: Outstanding Todos chip renders a dot the design omits — design's otChip is text-only ("4 OPEN")
status: Open
---

# Bug 083 — Outstanding Todos chip dot

**Status**: Open

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen`'s `otChip`
header pill has a single child: the `4 OPEN` text node. No dot
ellipse, unlike `drLive`, `ctLive`, etc., which place a dot
*before* their text.

The `cui-status-pill` template unconditionally renders a
`<span class="status-pill__dot">`, so the Outstanding Todos chip
shows a `[•] 4 OPEN` instead of the design's plain `4 OPEN`.

The fix is to give `cui-status-pill` an optional `[dot]` input
defaulting to `true` (preserving every other tile's behaviour),
and have outstanding-todos pass `[dot]="false"`.

## Affected files

- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.ts`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.html`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts`

## Reproduction

1. Open the dashboard at <http://localhost:4200>.
2. The Outstanding Todos pill shows a `•` dot before the count.
3. The corresponding `ui-design.pen` shows just `4 OPEN`.

## Expected

- `cui-status-pill` accepts `[dot]` input (boolean, default `true`).
- The dot `<span>` only renders when `dot()` is `true`.
- Outstanding-todos passes `[dot]="false"`.
- Every other consumer of `cui-status-pill` is unchanged (default
  preserves the existing two-element layout).

## Verification

- Unit (status-pill template source): assert template uses
  `@if (dot()) { … }` to gate the `.status-pill__dot` element.
- Unit (status-pill render): when `dot=false`, the rendered DOM
  has no `.status-pill__dot` element.
- Unit (outstanding-todos template): assert `<cui-status-pill
  …[dot]="false"…>` is present.
- All other tiles' specs (which don't pass `[dot]`) keep showing
  the dot via the default.
