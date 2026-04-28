---
id: bug-116
title: bug-114's todayPointGlow plugin uses magic numbers 7 and 8 instead of named constants
status: Fixed
---

# Bug 116 — todayPointGlow magic numbers

**Status**: Fixed

## Fix

Hoisted two named constants before the plugin:

- `TODAY_GLOW_RADIUS = 7` (matches dataset `POINT_RADIUS_HIGHLIGHT`)
- `TODAY_GLOW_BLUR = 8` (design `todayDot.effect.blur`)

Plugin body now reads `ctx.shadowBlur = TODAY_GLOW_BLUR` and
`ctx.arc(point.x, point.y, TODAY_GLOW_RADIUS, ...)`. Visual
output is identical; the named constants document intent and
prevent silent drift if the dataset's highlighted radius later
changes. The bug-114 spec was loosened to accept a constant
reference. 313/313 workspace tests green.

## Description

bug-114's `todayPointGlow` plugin contains two magic numbers:

```ts
ctx.shadowBlur = 8;            // ← design's effect.blur
ctx.arc(point.x, point.y, 7, 0, Math.PI * 2);  // ← matches POINT_RADIUS_HIGHLIGHT
```

The `7` is a duplication of the controller's
`POINT_RADIUS_HIGHLIGHT` (which sets the same point radius on
the dataset). If anyone changes the highlighted radius later,
the glow circle would silently misalign with the actual point.

The `8` is the design's `effect.blur` — naming it documents the
intent.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "shadowBlur\|ctx\.arc" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Two magic-number lines.

## Expected

Local named constants in the component file:

```ts
const TODAY_GLOW_RADIUS = 7;   // matches dataset POINT_RADIUS_HIGHLIGHT
const TODAY_GLOW_BLUR = 8;     // design todayDot effect.blur
```

Used in the plugin body. Refactor only — visual output is
identical.

## Verification

- Unit (TS source): the plugin body references named constants
  for the radius and blur values.
- All existing consistency-trend specs continue to pass.
