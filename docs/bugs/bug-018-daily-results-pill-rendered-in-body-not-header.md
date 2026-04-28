---
id: bug-018
title: Daily Results — LIVE status pill is rendered in the body slot, not in the header next to the title
status: Open
---

# Bug 018 — Daily Results pill in wrong slot (body instead of header)

**Status**: Open

## Description

The Daily Results tile design (`docs/tiles/daily-results-tile/ui-design.pen`,
frame `Daily-Results-Tile` → `drHd`) places the `LIVE` status pill on the **right
side of the header row**, on the same baseline as the `today` icon and the
`Daily Results` / `Today` title block. The header is a horizontal flex row
with a fill-container spacer between the title block and the pill.

The implementation in
`frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.html`
projects `<cui-status-pill>` as a child of `<commitments-tile-shell>`. The shell
template (`frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`)
has its single `<ng-content></ng-content>` inside `<mat-card-content>`, so the
pill ends up in the **body** of the card, on a separate row above the
metric value — not in the header.

In the rendered output the user therefore sees, top to bottom:

```
[icon] Daily Results            ← header
       TODAY (uppercase eyebrow)
─────── divider ───────         ← bug-019 (separate)
[● LIVE pill]                   ← should be in the header right
7 / 9
commitments completed
[progress bar]
```

The design instead requires:

```
[icon] Daily Results          [● LIVE pill]   ← single header row
       Today
7 / 9                                          ← centered body
commitments completed
[progress bar]
```

This same projection bug affects every plugin tile that uses
`<commitments-tile-shell>` and projects a pill — at minimum
`goal-metrics-tile.component.html` and `consistency-trend-tile.component.html`
follow the same pattern. The fix should be in the shell so that all tiles
gain a structurally correct header.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`
  — the shell needs a named slot (e.g. `<ng-content select="[tile-status]">`)
  inside `mat-card-header`, on the trailing flex side, so projected pills land
  in the header instead of the body.
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
  — already has `.tile-shell__header { justify-content: space-between }` so
  layout requires no new CSS once a named slot exists.
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.html`
  — add the slot attribute (e.g. `<cui-status-pill tile-status …>`).

## Reproduction

1. Run `npm start` in `frontend/`.
2. Open `http://localhost:4200/dashboard` and wait for tiles to render.
3. Locate the Daily Results tile.
4. Observe that the pulsing green `LIVE` pill sits **on its own row above the
   `7 / 9` value**, not on the right side of the header next to the title.
5. Compare to `docs/tiles/daily-results-tile/ui-design.pen` — design has the
   pill in the header on the right.

## Expected

The `LIVE` pill renders inside the header row, right-aligned (flex
`justify-content: space-between` between the title block and the pill), as
specified in the design.

## Suggested fix

In `tile-shell.component.html`, replace the existing single ng-content with
two named slots:

```html
<mat-card-header class="tile-shell__header">
  <div class="tile-shell__heading">
    …existing heading markup…
  </div>
  <ng-content select="[tile-status]"></ng-content>
</mat-card-header>

<mat-card-content class="tile-shell__body">
  <ng-content></ng-content>
</mat-card-content>
```

Then in `daily-results-tile.component.html`:

```html
<cui-status-pill
  tile-status
  [variant]="controller.mode()"
  [pulse]="controller.mode() === 'live'"
  [label]="statusLabel()"
></cui-status-pill>
```

Apply the same `tile-status` attribute to the `cui-status-pill` usage in
`goal-metrics-tile.component.html` and any other tile that wants the pill in
the header.

## Verification

- Unit: extend `tile-shell.component.spec.ts` with a test that projects a
  `<span tile-status>` and asserts the projected node lives inside the
  `mat-card-header`, not the body.
- E2E: a Playwright check that the bounding-box `top` of the `LIVE` pill is
  within the header's bounding-box top/bottom, not below it.
- Visual: screenshot the daily-results tile and compare to the .pen render.
