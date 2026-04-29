// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { DashboardBackendService, DashboardMode } from '@commitments/dashboard-framework';

export interface DailyResultsDto {
  mode: DashboardMode;
  asOf: string;
  date: string;
  completed: number;
  total: number;
}

@Injectable({ providedIn: 'root' })
export class DailyResultsService {
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<DailyResultsDto> {
    return this._backend.get<DailyResultsDto>('api/v1.0/commitments/daily-results', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
