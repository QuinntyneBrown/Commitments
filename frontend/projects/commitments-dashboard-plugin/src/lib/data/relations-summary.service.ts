// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { DashboardBackendService, DashboardMode } from '@commitments/dashboard-framework';

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
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<RelationsSummaryDto> {
    return this._backend.get<RelationsSummaryDto>('api/v1.0/relations/summary', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
