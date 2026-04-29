// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';

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
