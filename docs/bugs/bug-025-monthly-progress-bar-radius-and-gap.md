---
id: bug-025
title: Monthly Progress — bar corner-radius is 6px and bars gap is 10px; design is 4px and 12px
status: Open
---

# Bug 025 — Monthly Progress bar radius + gap drift

**Status**: Open

## Description

Two detail-level deviations from
`docs/tiles/monthly-progress-tile/ui-design.pen` that surfaced after
bug-024 added the week-labels row:

1. **Bar corner-radius.** Each `mpBar*` frame in the design declares
   `cornerRadius: [4, 4, 0, 0]` (top corners only, 4px). The
   implementation in `monthly-progress-tile.component.scss` styles
   `.bars span` with `border-radius: 6px 6px 0 0` — 2px rounder than
   the design.
2. **Bars gap.** The design's `mpBars` frame uses `gap: 12`. The
   implementation's `.bars` rule uses `gap: 10px`. The newly added
   `.bar-labels` row from bug-024 also uses `gap: 10px` to mirror the
   bars; both should move to `12px` together so the labels stay
   aligned.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`

## Reproduction

1. Render the Monthly Progress tile.
2. Inspect a `.bars span` — `border-radius` is `6px 6px 0 0`.
3. Inspect `.bars` — `gap` is `10px`.
4. Compare to the .pen — bars have `4px` top corners and the bar
   group is gap-12.

## Expected

- `.bars span { border-radius: 4px 4px 0 0; }`
- `.bars { gap: 12px; }`
- `.bar-labels { gap: 12px; }` (kept in sync so column centres line up)

## Verification

- Unit: source-level SCSS check — `.bars span` declares
  `border-radius: 4px 4px 0 0`, `.bars` declares `gap: 12px`,
  `.bar-labels` declares `gap: 12px`.
- Visual: screenshot the tile; bars should sit 2px wider apart with
  slightly tighter top corners.
