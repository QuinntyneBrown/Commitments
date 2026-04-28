---
id: bug-082
title: Outstanding Todos chip shows the dashboard mode ("LIVE"/"REVIEW") instead of "{count} OPEN" as the design specifies
status: Open
---

# Bug 082 — Outstanding Todos chip label

**Status**: Open

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen`'s `otChip` —
the warning-tinted pill in the header — has a single text node:

```
"4 OPEN" — Inter 11/700, fill #FFA726
```

That is, count + literal `" OPEN"`. The pill is communicating
*how many open todos there are*, not the dashboard's mode.

The implementation projects a `<cui-status-pill>` with
`[label]="statusLabel()"`, where `statusLabel` is computed as:

```ts
this.controller.mode().toUpperCase()
```

So the pill renders `LIVE` (or `REVIEW`) instead of `4 OPEN`.
That hides the count from the header and surfaces a piece of
information the design doesn't reserve any space for.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts`
  — replace `statusLabel` derivation
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts`
  — assert `statusLabel` is count-derived, not mode-derived

## Reproduction

1. Open the dashboard at <http://localhost:4200>.
2. The Outstanding Todos pill reads `LIVE`.
3. The corresponding `ui-design.pen` shows `4 OPEN`.

## Expected

The chip label is derived from `controller.count()`:

```ts
readonly statusLabel = computed(() => `${this.controller.count()} OPEN`);
```

So a count of 4 renders `4 OPEN`, count of 0 renders `0 OPEN`,
matching the design.

## Verification

- Unit (TS source): regex assertion that `statusLabel` is built
  from `controller.count()` and the literal `'OPEN'` — and **does
  not** read `mode()` or call `.toUpperCase()`.
- All existing outstanding-todos specs continue to pass.
