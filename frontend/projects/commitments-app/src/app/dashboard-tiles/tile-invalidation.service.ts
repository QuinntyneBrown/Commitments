// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { filter, share } from 'rxjs/operators';

import { HubClient } from '../core/hub-client';

export type DashboardTileDataset =
  | 'dailyResults'
  | 'weeklyFocus'
  | 'monthlyProgress'
  | 'outstandingTodos'
  | 'relations'
  | 'goalTrend';

export interface DashboardTileDataInvalidatedPayload {
  datasets: DashboardTileDataset[];
  affectedGoalIds: string[];
  affectedCommitmentIds: string[];
  affectedToDoIds: string[];
  from: string | null;
  to: string | null;
  reason: 'activityChanged' | 'commitmentChanged' | 'frequencyChanged' | 'toDoChanged' | 'profileChanged';
}

@Injectable({ providedIn: 'root' })
export class TileInvalidationService {
  private readonly _stream: Observable<DashboardTileDataInvalidatedPayload>;

  constructor(hub: HubClient = inject(HubClient)) {
    this._stream = hub
      .on<DashboardTileDataInvalidatedPayload>('dashboardTileDataInvalidated')
      .pipe(share());
  }

  invalidations$(dataset: DashboardTileDataset): Observable<DashboardTileDataInvalidatedPayload> {
    return this._stream.pipe(filter(p => p.datasets.includes(dataset)));
  }
}
