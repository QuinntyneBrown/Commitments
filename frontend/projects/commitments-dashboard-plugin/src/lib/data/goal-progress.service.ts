// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable, InjectionToken, Optional, inject } from '@angular/core';
import { Observable } from 'rxjs';

export const GOAL_PROGRESS_BASE_URL = new InjectionToken<string>('GOAL_PROGRESS_BASE_URL');

export interface GoalProgressDto {
  goalId: string;
  count: number;
  target: number;
  asOf: string;
}

export interface GoalProgressUpdated {
  goalId: string;
  count: number;
  asOf: string;
}

@Injectable({ providedIn: 'root' })
export class GoalProgressService {
  private readonly _client = inject(HttpClient);

  constructor(@Optional() @Inject(GOAL_PROGRESS_BASE_URL) private readonly _baseUrl: string | null) {}

  getCurrent(goalId: string): Observable<GoalProgressDto> {
    const params = new HttpParams().set('goalId', goalId);
    return this._client.get<GoalProgressDto>(`${this._baseUrl ?? ''}api/v1.0/goal-progress/current`, { params });
  }

  getAt(goalId: string, asOf: string): Observable<GoalProgressDto> {
    const params = new HttpParams()
      .set('goalId', goalId)
      .set('asOf', new Date(asOf).toISOString());
    return this._client.get<GoalProgressDto>(`${this._baseUrl ?? ''}api/v1.0/goal-progress/at`, { params });
  }
}
