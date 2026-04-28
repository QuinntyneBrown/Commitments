---
id: bug-119
title: ngAfterViewInit on consistency-trend-tile is ~57 lines mixing constants, helpers, plugin, and chart config
status: Open
---

# Bug 119 — extract chart config builder

**Status**: Open

## Description

After bugs 077/091/092/093/094/114/115/116/117/118 enriched
the chart config, `ngAfterViewInit` on
`ConsistencyTrendTileComponent` is ~57 lines:

- Controller alias
- TODAY_GLOW_RADIUS / TODAY_GLOW_BLUR constants
- isHighlighted helper
- todayPointGlow plugin (15 lines)
- ChartConfiguration object (33 lines)
- `_adapter.attach` call

A junior dev opening the file has to scan a wall of nested
config to find the lifecycle hook's actual job. Extracting the
config building into a private method shrinks `ngAfterViewInit`
to one statement and groups the chart-specific glue in its own
named function.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
awk '/ngAfterViewInit/,/^  \}/' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts | wc -l
```

Returns ~57 lines.

## Expected

```ts
ngAfterViewInit(): void {
  this._adapter.attach(this.plotRef, this._buildChartConfig(this.controller));
}

private _buildChartConfig(controller: ConsistencyTrendController): ChartConfiguration<'line'> {
  // TODAY_GLOW_*, isHighlighted, todayPointGlow, return {…}
}
```

## Verification

- Unit (TS source): assert `ngAfterViewInit` body is short
  (e.g. one statement) and a `_buildChartConfig` method exists.
- All existing consistency-trend specs continue to pass.
