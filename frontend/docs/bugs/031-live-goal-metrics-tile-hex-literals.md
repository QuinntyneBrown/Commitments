---
id: 031
title: live-goal-metrics-tile.component.scss hard-codes hex literals for colours that have --cui-* tokens
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 3b77365
flow: dashboard-layout
severity: low
---

# live-goal-metrics-tile.component.scss hard-codes hex literals for colours that have --cui-* tokens

## Summary

Same shape as bug 030 (review-scrubber). The
`live-goal-metrics-tile` style sheet declares three literals
that already match palette tokens:

| Selector | Literal | Token equivalent |
|---|---|---|
| `.bars__bar--today { background }` | `#FF4081` | `--cui-accent` |
| `.readout { color }` | `#B0B0B0` | `--cui-text-secondary` |
| `.readout__separator { color }` | `#666666` | `--cui-text-disabled` |

The tile only mounts when the user adds it via "Add Tile", so
this isn't visible in the default 5-tile layout — but the bars
flash pink when live, and the separator + readout sit beside
the metric. As soon as anyone tweaks the palette, the tile
silently drifts off-design.

## Reproduction

`grep -rn "background: #\|color: #" projects/commitments-app/src/app/components/live-goal-metrics-tile`
→ matches at lines 23, 29, 37.

## Fix outline (radically simple)

Three hex-to-token swaps in
`projects/commitments-app/src/app/components/live-goal-metrics-tile/live-goal-metrics-tile.component.scss`:

  - `.bars__bar--today { background: var(--cui-accent); }`
  - `.readout { color: var(--cui-text-secondary); }`
  - `.readout__separator { color: var(--cui-text-disabled); }`

The translucent grey
`background: rgba(102, 102, 102, 0.6)` on `.bars__bar` is left
as a literal — it's a translucency over surface, not a palette
colour, and pulling it through `color-mix()` would broaden
scope.

## Tests to add (failing first)

Existing `live-goal-metrics-controller.spec` exercises the
controller. The visible bug here is purely token alignment — no
e2e regression net is needed beyond what the tile-rendering
tests already provide. Bug log + fix in one commit pair.
