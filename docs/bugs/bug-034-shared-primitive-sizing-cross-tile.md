---
id: bug-034
title: Tile shell title is 18px and status-pill dot is 6×6 with 0.6px tracking; every tile design specifies 16px / 8×8 / 1px
status: Fixed
---

# Bug 034 — Shared primitive sizing drift across all tiles

**Status**: Fixed

## Fix

Three single-property edits propagating to every plugin tile:
- `.tile-shell__title` font-size: 18px → 16px
- `.status-pill__dot` width/height: 6px → 8px
- `.status-pill` letter-spacing: 0.6px → 1px

Coverage:
- New spec `renders the title at 16px to match every tile design`
  in `tile-shell.component.spec.ts`.
- New specs `renders the dot at 8x8` and `uses 1px letter-spacing
  on the pill text` in `status-pill.component.spec.ts`.
- All 20 affected suites pass (93/93 — was 90/90 before).

## Description

Three detail-level deviations in shared primitives that propagate to
every tile:

1. **Tile-shell title**. Each tile design (`ctTitle`, `drTitle`,
   `wfTitle`, `mpTitle`, `otTitle`, `rlTitle`, `j3rVYG`, etc.)
   specifies the title at Inter 16 / 500. The implementation in
   `tile-shell.component.scss` styles `.tile-shell__title` at
   `font-size: 18px`.
2. **Status-pill dot**. Every tile design that includes a pill
   (`ctLive`, `drLive`, `otChip`, `noWds`) draws an 8×8 dot. The
   implementation styles `.status-pill__dot` at `6px × 6px`.
3. **Status-pill letter spacing**. Every design's `LIVE` text
   declares `letterSpacing: 1`. The implementation uses
   `letter-spacing: 0.6px`.

None are individually critical, but together they shift every tile's
header subtly off its mark.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss`

## Reproduction

1. Render any tile (e.g. Daily Results).
2. Inspect `.tile-shell__title` — `font-size` is `18px`.
3. Inspect `.status-pill__dot` — `width`/`height` is `6px`.
4. Inspect `.status-pill` — `letter-spacing` is `0.6px`.
5. Compare to the .pen — title 16px, dot 8×8, letter-spacing 1px.

## Expected

- `.tile-shell__title { font-size: 16px; }`
- `.status-pill__dot { width: 8px; height: 8px; }`
- `.status-pill { letter-spacing: 1px; }`

## Verification

- Unit: source-level SCSS specs in the existing tile-shell and
  status-pill spec files asserting each of the above values.
- Visual: screenshot any tile vs the .pen.
