---
id: bug-019
title: Tile shell — eyebrow renders above the title in uppercase/bold; design has it below the title in sentence case
status: Fixed
---

# Bug 019 — Eyebrow position + case wrong on tile shell

**Status**: Fixed

## Fix

`tile-shell.component.html` now renders `.tile-shell__title-row` first
inside `.tile-shell__heading`, with the optional `<p
class="tile-shell__eyebrow">` immediately after. `tile-shell.component.scss`
drops `text-transform: uppercase` and switches `font-weight` from `700`
to `400`, with `margin: 2px 0 0` to mirror the design's vertical-frame
gap of 2.

Coverage:
- New unit spec `renders the eyebrow after the title-row in DOM order`
  asserts the rebuilt order via `compareDocumentPosition`.
- All 10 affected suites pass (42/42 — was 41/41 before this change).
- Visual case+weight remains a Playwright concern listed in this doc's
  verification plan.

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
