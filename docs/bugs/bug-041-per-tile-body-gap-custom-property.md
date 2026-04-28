---
id: bug-041
title: Tile shell body padding is hard-coded at 10px; weekly-focus / monthly-progress / relations designs each specify 12 (and consistency-trend 14) — needs per-tile customization
status: Fixed
---

# Bug 041 — Tile body padding-top is fixed at 10px instead of per-tile-design

**Status**: Fixed

## Fix

`tile-shell.component.scss`:
- `.tile-shell__body { padding-top: var(--cui-tile-body-gap, 10px); }`
- Removed `.tile-shell--chart .tile-shell__body { padding-top: 14px; }`
  (bug-040 mechanism replaced by per-tile property).

Per-tile opt-ins (`:host { --cui-tile-body-gap: …; }`):
- weekly-focus, monthly-progress, relations: 12px
- consistency-trend: 14px

Daily-results and outstanding-todos keep the default 10px (no
override needed — they already match the .pen).

The 12px literal is duplicated across three tiles intentionally;
promote to a shared token only when cross-cutting demand emerges.

Coverage:
- `tile-shell.component.spec.ts` asserts `.tile-shell__body`
  consumes `var(--cui-tile-body-gap)` and the chart-variant override
  is gone.
- weekly-focus, monthly-progress, relations specs each assert their
  `:host` declares `--cui-tile-body-gap: 12px`.
- consistency-trend spec asserts `:host` declares
  `--cui-tile-body-gap: 14px`.
- All 20 affected suites pass (109/109 — was 104/104 before).

## Description

The .pen tile-frame `gap` (vertical spacing between header and body
content) varies across the six tile designs:

| Tile                | .pen tile gap | Current effective gap |
| ------------------- | ------------- | --------------------- |
| daily-results       | 10            | 10 ✓                  |
| outstanding-todos   | 10            | 10 ✓                  |
| weekly-focus        | 12            | 10 ✗                  |
| monthly-progress    | 12            | 10 ✗                  |
| relations           | 12            | 10 ✗                  |
| consistency-trend   | 14            | 14 ✓ (via bug-040)    |

Bug-020 set `.tile-shell__body { padding-top: 10px }`. Bug-040
added a chart-variant override
`.tile-shell--chart .tile-shell__body { padding-top: 14px }`.
Three tiles are still 2px short.

The variant mechanism doesn't separate "12px" from "10px" cleanly
because all five non-chart tiles share `variant="metric"`. A per-
tile customization slot is the right shape.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss` (refactor away from `.tile-shell--chart` override)

## Reproduction

1. Render any of weekly-focus / monthly-progress / relations.
2. Measure header→body gap — 10px in DOM.
3. Compare to .pen — 12px.

## Expected

`tile-shell.component.scss`:

```scss
.tile-shell__body {
  …
  padding-top: var(--cui-tile-body-gap, 10px);
}
/* Drop the .tile-shell--chart override — replaced by host custom property */
```

Each per-tile SCSS sets the host-level value:

- `weekly-focus`, `monthly-progress`, `relations`:
  ```scss
  :host { --cui-tile-body-gap: 12px; }
  ```
- `consistency-trend`:
  ```scss
  :host { --cui-tile-body-gap: 14px; }
  ```

Daily-results and outstanding-todos keep the default 10px (no
override needed).

## Verification

- Unit:
  - `tile-shell.component.spec.ts` asserts
    `.tile-shell__body` declares
    `padding-top: var(--cui-tile-body-gap` and the
    `.tile-shell--chart` body padding-top override no longer exists
    (bug-040's check moves to per-tile assertion).
  - `weekly-focus-tile`, `monthly-progress-tile`, `relations-tile`
    specs each assert `:host` declares
    `--cui-tile-body-gap: 12px`.
  - `consistency-trend-tile` spec asserts `:host` declares
    `--cui-tile-body-gap: 14px`.
- Visual: each tile renders with the correct .pen gap.
