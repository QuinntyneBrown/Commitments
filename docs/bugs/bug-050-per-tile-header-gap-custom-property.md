---
id: bug-050
title: Tile-shell title-row hard-codes 8px gap; consistency-trend design specifies 10px (matching its larger icon/title scale)
status: Fixed
---

# Bug 050 — Tile-shell title-row gap is fixed at 8px instead of per-tile-design

**Status**: Fixed

## Fix

`tile-shell.component.scss`:
- `.tile-shell__title-row { gap: var(--cui-tile-header-gap, 8px); }`

`consistency-trend-tile.component.scss`:
- `:host` adds `--cui-tile-header-gap: 10px`.

Other five tiles inherit the 8px default.

Tile-shell now exposes four per-tile customization slots:
- `--cui-tile-body-gap` (bug-041)
- `--cui-tile-header-gap` (bug-050)
- `--cui-tile-icon-color` (bug-048)
- `--cui-tile-icon-size` (bug-049)

Coverage:
- `tile-shell.component.spec.ts` asserts `.tile-shell__title-row`
  consumes `var(--cui-tile-header-gap)`.
- `consistency-trend-tile.component.spec.ts` asserts `:host`
  declares `--cui-tile-header-gap: 10px`.
- All 20 affected suites pass (123/123 — was 121/121 before).

## Description

`.tile-shell__title-row` controls the gap between the icon and the
title text within a tile header. Each tile design's header frame
specifies its own gap:

| Tile               | .pen header gap | Current |
| ------------------ | --------------- | ------- |
| daily-results      | 8               | 8 ✓     |
| weekly-focus       | 8               | 8 ✓     |
| monthly-progress   | 8               | 8 ✓     |
| outstanding-todos  | 8               | 8 ✓     |
| relations          | 8               | 8 ✓     |
| consistency-trend  | **10**          | 8 ✗     |

Five of six tiles match. Consistency-trend's design header
(`ctHd` frame, `gap: 10`) gives a slightly roomier separation
between the 22×22 icon and the 16px title — proportional to its
larger overall tile scale (720×420 vs ~320×200 for the others).

Following the pattern set by bug-041 (body gap), bug-048 (icon
colour), and bug-049 (icon size), the fix introduces a
`--cui-tile-header-gap` custom property with `8px` as the
fallback. Consistency-trend opts in to `10px`.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect `.tile-shell__title-row` — `gap: 8px`.
3. Compare to the .pen — gap is 10px.

## Expected

```scss
.tile-shell__title-row {
  display: flex;
  align-items: center;
  gap: var(--cui-tile-header-gap, 8px);
}
```

Consistency-trend host:

```scss
:host {
  …
  --cui-tile-header-gap: 10px;
}
```

## Verification

- Unit:
  - `tile-shell.component.spec.ts` asserts
    `.tile-shell__title-row` declares
    `gap: var(--cui-tile-header-gap`.
  - `consistency-trend-tile.component.spec.ts` asserts `:host`
    declares `--cui-tile-header-gap: 10px`.
- Visual: the consistency-trend header has a touch more space
  between its icon and title; the other five tiles render
  unchanged.
