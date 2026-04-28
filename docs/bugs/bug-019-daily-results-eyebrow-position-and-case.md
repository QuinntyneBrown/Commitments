---
id: bug-019
title: Tile shell — eyebrow renders above the title in uppercase/bold; design has it below the title in sentence case
status: Open
---

# Bug 019 — Eyebrow position + case wrong on tile shell

**Status**: Open

## Description

The Daily Results design (`docs/tiles/daily-results-tile/ui-design.pen`,
frame `drTitle`, vertical layout with gap 2) renders the title block as:

```
Daily Results       (Inter 16 / 500 / #FFFFFF)
Today               (Inter 11 / 400 / #B0B0B0)   ← below, sentence case
```

The current `tile-shell.component.html` instead produces:

```
TODAY               (eyebrow, uppercased, bold 700)   ← above
[icon] Daily Results
```

Two divergences:

1. **Order** — the eyebrow precedes `.tile-shell__title-row` in the DOM;
   the design renders it after.
2. **Typography** — `.tile-shell__eyebrow` applies
   `text-transform: uppercase` and `font-weight: 700`, so `"Today"`
   becomes `"TODAY"` and is rendered bold. The design has the eyebrow
   as plain sentence-case `"Today"` at default weight.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

1. Run the dashboard with the Daily Results tile present.
2. Observe `TODAY` (uppercase, bold) above `[icon] Daily Results`.
3. Compare to the .pen — design shows `Daily Results` on top, `Today`
   below in sentence case.

## Expected

The header reads:

```
[icon today]  Daily Results
              Today
```

with `"Today"` in `#B0B0B0`, font-size 11, weight 400, no uppercase.

## Verification

- Unit: `tile-shell.component.spec.ts` — assert eyebrow is rendered
  *after* the title-row in DOM order and that its computed
  `text-transform` is `none` and `font-weight` ≤ 500.
- Visual: screenshot the daily-results tile; compare to the .pen.
