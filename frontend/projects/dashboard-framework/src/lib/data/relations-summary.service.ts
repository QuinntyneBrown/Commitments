import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { DashboardMode } from '../tile-registration/tile.model';

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
