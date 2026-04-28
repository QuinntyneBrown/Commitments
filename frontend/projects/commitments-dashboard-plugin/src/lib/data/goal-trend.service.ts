// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';
import { Observable } from 'rxjs';

export const TREND_BASE_URL = new InjectionToken<string>('TREND_BASE_URL');

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
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(TREND_BASE_URL, { optional: true });

  getTrend(
    goalId: string,
    mode: DashboardMode,
    asOf: string | null,
    windowDays: number = 30
  ): Observable<GoalTrendDto> {
    let params = new HttpParams()
      .set('goalId', goalId)
      .set('windowDays', String(windowDays));

    if (mode === 'review' && asOf) {
      params = params.set('asOf', new Date(asOf).toISOString());
    }

    const url = `${this._baseUrl ?? ''}api/v1.0/goal-progress/trend`;
    return this._client.get<GoalTrendDto>(url, { params });
  }
}
