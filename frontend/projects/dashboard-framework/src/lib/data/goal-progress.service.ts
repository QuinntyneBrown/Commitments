import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';

export interface GoalProgressDto {
  goalId: string;
  count: number;
  target: number;
  asOf: string;
}

@Injectable({ providedIn: 'root' })
export class GoalProgressService {
  private readonly _backend = inject(DashboardBackendService);

  getCurrent(goalId: string): Promise<GoalProgressDto> {
    return this._backend.get<GoalProgressDto>('api/v1.0/goal-progress/current', { goalId });
  }

  getAt(goalId: string, asOf: string): Promise<GoalProgressDto> {
    return this._backend.get<GoalProgressDto>('api/v1.0/goal-progress/at', {
      goalId,
      asOf: new Date(asOf)
    });
  }
}
