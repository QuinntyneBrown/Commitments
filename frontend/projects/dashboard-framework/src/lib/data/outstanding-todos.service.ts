import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { DashboardMode } from '../tile-registration/tile.model';

export interface OutstandingTodosDto {
  mode: DashboardMode;
  asOf: string;
  count: number;
  todayCount?: number;
}

@Injectable({ providedIn: 'root' })
export class OutstandingTodosService {
  private readonly _backend = inject(DashboardBackendService);

  get(mode: DashboardMode, asOf: string | null): Promise<OutstandingTodosDto> {
    return this._backend.get<OutstandingTodosDto>('api/v1.0/todos/outstanding-count', {
      asOf: mode === 'review' && asOf ? new Date(asOf) : null,
      includeToday: mode === 'review' && !!asOf ? true : null
    });
  }
}
