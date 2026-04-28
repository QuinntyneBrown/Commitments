// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';
import { Observable } from 'rxjs';

export const MONTHLY_PROGRESS_BASE_URL = new InjectionToken<string>('MONTHLY_PROGRESS_BASE_URL');

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
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(MONTHLY_PROGRESS_BASE_URL, { optional: true });

  get(asOf: string | null): Observable<MonthlyProgressDto> {
    let params = new HttpParams();
    if (asOf) {
      params = params.set('asOf', new Date(asOf).toISOString());
    }
    const url = `${this._baseUrl ?? ''}api/v1.0/monthly-progress`;
    return this._client.get<MonthlyProgressDto>(url, { params });
  }
}
