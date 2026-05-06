import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { DashboardMode } from '../tile-registration/tile.model';

export interface GoalTrendPointDto {
  date: string;
  completed: number;
  target: number;
  percentage: number;
}

export interface GoalTrendDto {
  goalId: string;
  mode: DashboardMode;
  asOf: string;
  windowDays: number;
  points: GoalTrendPointDto[];
  currentPercentage: number;
  peakPercentage: number;
  lowPercentage: number;
  deltaLabel: string;
}

@Injectable({ providedIn: 'root' })
export class GoalTrendService {
  private readonly _backend = inject(DashboardBackendService);

  getTrend(
    goalId: string,
    mode: DashboardMode,
    asOf: string | null,
    windowDays: number = 30
  ): Promise<GoalTrendDto> {
    return this._backend.get<GoalTrendDto>('api/v1.0/goal-progress/trend', {
      goalId,
      windowDays,
      asOf: mode === 'review' && asOf ? new Date(asOf) : null
    });
  }
}
