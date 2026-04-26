// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { baseUrl } from '../core/constants';

export interface GoalProgress {
  goalId: string;
  target: number;
  count: number;
  asOf: Date | string;
}

@Injectable({ providedIn: 'root' })
export class GoalProgressService {
  private readonly _baseUrl = inject(baseUrl as any) as string;
  private readonly _client = inject(HttpClient);

  getCurrent(goalId: string): Observable<GoalProgress> {
    const params = new HttpParams().set('goalId', goalId);
    return this._client.get<GoalProgress>(`${this._baseUrl}api/v1.0/goal-progress/current`, { params });
  }
}
