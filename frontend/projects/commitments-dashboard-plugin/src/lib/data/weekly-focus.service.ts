// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { DashboardBackendService, DashboardMode } from '@commitments/dashboard-framework';

export interface WeeklyFocusItemDto {
  name: string;
  supportingMetric: string;
  rank: number;
}

export interface WeeklyFocusDto {
  mode: DashboardMode;
  asOf: string;
  weekStart: string;
  weekEnd: string;
  focusAreas: WeeklyFocusItemDto[];
}

@Injectable({ providedIn: 'root' })
export class WeeklyFocusService {
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<WeeklyFocusDto> {
    return this._backend.get<WeeklyFocusDto>('api/v1.0/weekly-focus', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
