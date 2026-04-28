---
id: bug-019
title: Daily Results — "Today" eyebrow is rendered above the title in uppercase, but design has it below the title in sentence case
status: Open
---

# Bug 019 — Eyebrow position + case wrong on Daily Results title

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

Two divergences from the design:

1. **Order** — the eyebrow is rendered *above* the title-row in the shell
   (`.tile-shell__eyebrow` precedes `.tile-shell__title-row` in the DOM); the
   design renders it *below* the title.
2. **Typography** — the shell applies `text-transform: uppercase` and
   `font-weight: 700` to `.tile-shell__eyebrow`, so `"Today"` becomes
   `"TODAY"` and is rendered bold. The design has the eyebrow as plain
   sentence-case `"Today"` at default weight (400).

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`
  — move the `<p class="tile-shell__eyebrow">` to *after* `.tile-shell__title-row`.
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
  — drop `text-transform: uppercase`, drop `font-weight: 700` (default to
  400), and tighten the spacing to `margin: 2px 0 0` (gap 2 in the design)
  rather than `margin: 0 0 4px`.

## Reproduction

1. Run the dashboard with the Daily Results tile present.
2. Observe the title area: an uppercase bold `TODAY` sits **above** an
   icon + `Daily Results` row.
3. Open `docs/tiles/daily-results-tile/ui-design.pen` (or use
   `mcp__pencil__get_screenshot` on node `sXIl5`) and compare — design
   shows `Daily Results` on top, `Today` below in sentence case.

## Expected

The header reads:

```
[icon today]  Daily Results
              Today
```

with `"Today"` in `#B0B0B0`, font-size 11, weight 400, no uppercase.

## Suggested fix

`tile-shell.component.html`:

```html
<div class="tile-shell__heading">
  <div class="tile-shell__title-row">
    @if (icon()) {
      <span class="material-symbols-rounded tile-shell__icon">{{ icon() }}</span>
    }
    <mat-card-title class="tile-shell__title" data-testid="tile-title">{{ title() }}</mat-card-title>
  </div>
  @if (eyebrow()) {
    <p class="tile-shell__eyebrow">{{ eyebrow() }}</p>
  }
</div>
```

`tile-shell.component.scss`:

```scss
.tile-shell__eyebrow {
  margin: 2px 0 0;
  color: var(--cui-text-secondary, #B0B0B0);
  font-size: 11px;
  font-weight: 400;
  // text-transform: uppercase;  // removed
}
```

## Verification

- Unit: `tile-shell.component.spec.ts` — assert the eyebrow element comes
  *after* the title-row in DOM order, and that its computed
  `text-transform` is `none` and `font-weight` <= 500.
- Visual: screenshot the daily-results tile, compare to `ui-design.pen`.
