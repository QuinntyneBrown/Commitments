import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { DashboardMode } from '../tile-registration/tile.model';

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
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<WeeklyFocusDto> {
    return this._backend.get<WeeklyFocusDto>('api/v1.0/weekly-focus', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
