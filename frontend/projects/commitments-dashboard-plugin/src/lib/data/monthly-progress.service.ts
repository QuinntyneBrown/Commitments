// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { DashboardBackendService, DashboardMode } from '@commitments/dashboard-framework';

export interface MonthlyProgressBucketDto {
  weekStart: string;
  weekEnd: string;
  completed: number;
  target: number;
  percentage: number;
}

export interface MonthlyProgressDto {
  mode: DashboardMode;
  asOf: string;
  windowDays: number;
  buckets: MonthlyProgressBucketDto[];
  isEmpty: boolean;
}

@Injectable({ providedIn: 'root' })
export class MonthlyProgressService {
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<MonthlyProgressDto> {
    return this._backend.get<MonthlyProgressDto>('api/v1.0/monthly-progress', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
