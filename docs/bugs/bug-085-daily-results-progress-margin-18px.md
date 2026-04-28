---
id: bug-085
title: Daily Results progress bar uses 18px margin where the design specifies a 10px gap
status: Open
---

# Bug 085 — Daily Results progress bar margin

**Status**: Open

## Description

`docs/tiles/daily-results-tile/ui-design.pen` (frame `sXIl5`) is
laid out as:

- Tile root: vertical, `padding: 16, gap: 10`
- Three direct children, in order:
  1. `drHd` — header (icon + title + LIVE pill)
  2. `drBody` — `height: fill_container`, vertically centered
     "7 / 9" + "commitments completed"
  3. `drTrack` — 8px-tall progress track

So the spacing between visible-content sections is **10px both
above and below the body**: 10px between header and body, and
10px between body and track.

`daily-results-tile.component.scss:26` instead pins:

```scss
.progress { margin-top: 18px; }
```

Combined with `flex: 1 1 auto` on `.metric`, this puts an extra
8px between the body and the progress bar than the design
specifies. The header-to-body gap is already 10px (handled by
`tile-shell`'s `padding-top: var(--cui-tile-body-gap, 10px)`),
so the asymmetry is jarring.

The correct value is `10px`, matching the design's root gap and
the tile-body-gap token.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.spec.ts`

## Reproduction

```bash
grep -n "margin-top\s*:\s*18px" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss
```

Returns one match.

## Expected

`.progress { margin-top: 10px }` (matching the design root gap
of 10 and the tile-body-gap token).

## Verification

- Unit (CSS source): assert `.progress` block contains
  `margin-top: 10px` (and not `18px`).
- All existing daily-results specs continue to pass.
