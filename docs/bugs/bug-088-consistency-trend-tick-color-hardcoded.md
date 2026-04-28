---
id: bug-088
title: consistency-trend chart's x-axis tick color uses literal '#666666' instead of the TEXT_MUTED design-system token
status: Open
---

# Bug 088 — chart tick color hardcoded hex

**Status**: Open

## Description

`consistency-trend-tile.component.ts:108` configures the
non-highlighted x-axis tick color with a literal hex:

```ts
ticks: {
  color: (ctx) => isLastTick(ctx) ? ACCENT_CHART : '#666666',
  ...
}
```

`ACCENT_CHART` is imported from `@commitments/ui` (the canvas
mirror of the SCSS palette). The very same token module exports
`TEXT_MUTED = '#666666'` for exactly this case, but the literal
`'#666666'` is used instead. If the design system ever rotates
its muted-text colour, this chart would silently drift.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "'#666666'" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Returns one match.

## Expected

The literal is replaced with `TEXT_MUTED` imported from
`@commitments/ui` — same pattern as the existing `ACCENT_CHART`
import a few lines above.

## Verification

- Unit (TS source): assert `TEXT_MUTED` is imported and used in
  the tick color callback (and the literal `'#666666'` is gone).
- All existing consistency-trend specs continue to pass.
