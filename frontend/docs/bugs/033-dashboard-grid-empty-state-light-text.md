---
id: 033
title: Dashboard empty-state still hard-codes light-theme text colours - invisible if a user empties the layout
status: open
discovered: 2026-04-27
flow: dashboard-layout
severity: medium
---

# Dashboard empty-state still hard-codes light-theme text colours — invisible if a user empties the layout

## Summary

`dashboard-grid.component.scss` declares two text-colour
literals on the empty-state pane:

```scss
.dashboard-grid__empty   { color: #516178; }     /* slate */
.dashboard-grid__empty h2 { color: #1d2a3d; }    /* near-black */
```

Both are leftover light-theme values. The pane only renders when
the user removes every tile (or the persisted layout JSON ends
up empty) — at the default 5-tile layout it isn't visible. But
once it does render against the dark `--cui-bg`, the slate body
text washes out and the "Your dashboard is empty" heading goes
near-black-on-near-black, i.e. **invisible**.

This is the last component in `dashboard-framework/src` with
hex literals (per `grep -rn`).

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Toggle "Edit Layout" → click the remove button on every
   tile until the grid is empty.
3. Inspect `.dashboard-grid__empty h2` and read its computed
   `color`.

**Expected:** `rgb(255, 255, 255)` (`--cui-text-primary`).
**Actual:** `rgb(29, 42, 61)` — near-black on dark.

## Fix outline (radically simple)

Two hex-to-token swaps in
`projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`:

  - `.dashboard-grid__empty   { color: var(--cui-text-secondary); }`
  - `.dashboard-grid__empty h2 { color: var(--cui-text-primary); }`

## Tests to add (failing first)

The state is a removed-all-tiles edge case — it's not exercised
by the existing dashboard suite at the default layout. The fix
is value-correcting (not value-preserving) but visible only in
that edge state. Bug log + fix in one commit pair, with a unit
assertion would require driving the layout store to empty;
that's heavier than the fix. Skip the failing test scaffold and
ship the swap.
