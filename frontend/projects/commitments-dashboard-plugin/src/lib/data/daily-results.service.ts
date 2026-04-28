// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';
import { Observable } from 'rxjs';

export const DAILY_RESULTS_BASE_URL = new InjectionToken<string>('DAILY_RESULTS_BASE_URL');

export interface DailyResultsDto {
  mode: DashboardMode;
  asOf: string;
  date: string;
  completed: number;
  total: number;
}

@Injectable({ providedIn: 'root' })
export class DailyResultsService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(DAILY_RESULTS_BASE_URL, { optional: true });

  get(asOf: string | null): Observable<DailyResultsDto> {
    let params = new HttpParams();
    if (asOf) {
      params = params.set('asOf', new Date(asOf).toISOString());
    }
    const url = `${this._baseUrl ?? ''}api/v1.0/commitment/daily-results`;
    return this._client.get<DailyResultsDto>(url, { params });
  }
}
