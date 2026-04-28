import { Provider, Type } from '@angular/core';
import { PLUGIN_TILES } from '@commitments/dashboard-framework';
import {
  ConsistencyTrendTileComponent,
  DailyResultsTileComponent,
  GoalMetricsTileComponent,
  MonthlyProgressTileComponent,
  OutstandingTodosTileComponent,
  RelationsTileComponent,
  WeeklyFocusTileComponent
} from './tiles';

export const COMMITMENTS_DASHBOARD_TILES: Type<unknown>[] = [
  DailyResultsTileComponent,
  WeeklyFocusTileComponent,
  MonthlyProgressTileComponent,
  OutstandingTodosTileComponent,
  RelationsTileComponent,
  ConsistencyTrendTileComponent,
  GoalMetricsTileComponent
];

export function provideCommitmentsDashboardPlugin(): Provider[] {
  return [{ provide: PLUGIN_TILES, useValue: COMMITMENTS_DASHBOARD_TILES, multi: true }];
}
