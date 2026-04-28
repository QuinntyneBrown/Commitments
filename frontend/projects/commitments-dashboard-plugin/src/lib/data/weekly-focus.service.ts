// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';
import { Observable } from 'rxjs';

export const WEEKLY_FOCUS_BASE_URL = new InjectionToken<string>('WEEKLY_FOCUS_BASE_URL');

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
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(WEEKLY_FOCUS_BASE_URL, { optional: true });

  get(asOf: string | null): Observable<WeeklyFocusDto> {
    let params = new HttpParams();
    if (asOf) {
      params = params.set('asOf', new Date(asOf).toISOString());
    }
    const url = `${this._baseUrl ?? ''}api/v1.0/weekly-focus`;
    return this._client.get<WeeklyFocusDto>(url, { params });
  }
}
