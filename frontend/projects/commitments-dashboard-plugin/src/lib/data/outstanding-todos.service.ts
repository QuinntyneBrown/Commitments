// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';
import { Observable } from 'rxjs';

export const OUTSTANDING_TODOS_BASE_URL = new InjectionToken<string>('OUTSTANDING_TODOS_BASE_URL');

export interface OutstandingTodosDto {
  mode: DashboardMode;
  asOf: string;
  count: number;
  todayCount?: number;
}

@Injectable({ providedIn: 'root' })
export class OutstandingTodosService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(OUTSTANDING_TODOS_BASE_URL, { optional: true });

  get(mode: DashboardMode, asOf: string | null): Observable<OutstandingTodosDto> {
    let params = new HttpParams();
    if (mode === 'review' && asOf) {
      params = params.set('asOf', new Date(asOf).toISOString()).set('includeToday', 'true');
    }
    const url = `${this._baseUrl ?? ''}api/v1.0/todos/outstanding-count`;
    return this._client.get<OutstandingTodosDto>(url, { params });
  }
}
