import { Provider, Type } from '@angular/core';
import { PLUGIN_TILES } from '@commitments/dashboard-framework';
import {
  DailyResultsTileComponent,
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
  RelationsTileComponent
];

export function provideCommitmentsDashboardPlugin(): Provider[] {
  return [{ provide: PLUGIN_TILES, useValue: COMMITMENTS_DASHBOARD_TILES, multi: true }];
}
