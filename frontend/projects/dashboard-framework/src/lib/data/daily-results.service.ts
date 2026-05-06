import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { DashboardMode } from '../tile-registration/tile.model';

export interface DailyResultsDto {
  mode: DashboardMode;
  asOf: string;
  date: string;
  completed: number;
  total: number;
}

@Injectable({ providedIn: 'root' })
export class DailyResultsService {
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<DailyResultsDto> {
    return this._backend.get<DailyResultsDto>('api/v1.0/commitments/daily-results', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
