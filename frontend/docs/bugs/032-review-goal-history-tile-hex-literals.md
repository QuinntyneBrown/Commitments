---
id: 032
title: review-goal-history-tile.component.scss hard-codes hex literals (incl. off-palette navy on date badge)
status: open
discovered: 2026-04-27
flow: dashboard-modes
severity: low
---

# review-goal-history-tile.component.scss hard-codes hex literals (incl. off-palette navy on date badge)

## Summary

Three hex literals in
`projects/commitments-app/src/app/components/review-goal-history-tile/review-goal-history-tile.component.scss`:

| Selector | Literal | Status |
|---|---|---|
| `.date-badge { color }` | `#5566a6` | **off-palette navy** |
| `.readout { color }` | `#B0B0B0` | matches `--cui-text-secondary` |
| `.readout__separator { color }` | `#666666` | matches `--cui-text-disabled` |

The date badge background is already
`rgba(159, 168, 218, 0.18)` (translucent `--cui-primary`); the
text colour should join the same primary token instead of
sitting at off-palette navy.

This tile mounts when the user adds **Review Goal History** in
review mode.

## Reproduction

`grep -rn "color: #" projects/commitments-app/src/app/components/review-goal-history-tile`
→ matches at lines 7, 19, 27.

## Fix outline (radically simple)

Three hex-to-token swaps in the existing rules:

  - `.date-badge { color: var(--cui-primary); }`
  - `.readout { color: var(--cui-text-secondary); }`
  - `.readout__separator { color: var(--cui-text-disabled); }`

The translucent badge background
(`rgba(159, 168, 218, 0.18)`) is intentionally left as a
literal — same translucency-over-surface pattern noted in
bug 030.

## Tests to add (failing first)

Existing `review-goal-history-controller.spec` covers the
controller behaviour. The change is value-preserving for the
two grey tokens and brings the date-badge text on-palette
(visual change from navy to lavender that's only visible when
the tile is mounted in review mode). Bug log + fix in one
commit pair.
