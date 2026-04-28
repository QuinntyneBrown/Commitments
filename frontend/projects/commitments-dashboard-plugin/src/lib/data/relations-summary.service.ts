// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';
import { Observable } from 'rxjs';

export const RELATIONS_BASE_URL = new InjectionToken<string>('RELATIONS_BASE_URL');

export interface RelationsSummaryItemDto {
  behaviourTypeId: string;
  name: string;
  count: number;
  percentage: number;
}

export interface RelationsSummaryDto {
  mode: DashboardMode;
  asOf: string;
  totalCommitments: number;
  relations: RelationsSummaryItemDto[];
}

@Injectable({ providedIn: 'root' })
export class RelationsSummaryService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(RELATIONS_BASE_URL, { optional: true });

  get(asOf: string | null): Observable<RelationsSummaryDto> {
    let params = new HttpParams();
    if (asOf) {
      params = params.set('asOf', new Date(asOf).toISOString());
    }
    const url = `${this._baseUrl ?? ''}api/v1.0/relations/summary`;
    return this._client.get<RelationsSummaryDto>(url, { params });
  }
}
