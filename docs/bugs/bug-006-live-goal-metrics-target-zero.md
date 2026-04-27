# Bug 006 — Live Goal Metrics tile shows target = 0 instead of 30

**Status**: Fixed

## Description

The `LiveGoalMetricsTile` renders with `target = 0` because the controller's `load()` is never called. The tile is missing `ngOnInit()` and a default `goalId` input, unlike the `ConsistencyTrendTile` which bootstraps with `@Input() goalId = 'demo-goal'` and calls `controller.load()` in `ngOnInit()`.

## Root Cause

`LiveGoalMetricsTileComponent` doesn't call `controller.load()`. The controller initializes with `target = signal(0)` and never gets data. The `LiveGoalMetricsController` also lacks a demo path for `goalId === 'demo-goal'` that bypasses the real API call.

## Affected files

- `frontend/projects/commitments-app/src/app/components/live-goal-metrics-tile/live-goal-metrics-tile.component.ts`
- `frontend/projects/commitments-app/src/app/components/live-goal-metrics-tile/live-goal-metrics-controller.ts`

## Fix

1. Add `@Input() goalId = 'demo-goal'` and `ngOnInit()` to the tile component that calls `controller.load(this.goalId)`.
2. Add a demo path in the controller: when `goalId === 'demo-goal'`, set `target = 30` and `count = 0` without making an HTTP call.
