import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { DashboardMode } from '../tile-registration/tile.model';

export interface MonthlyProgressBucketDto {
  weekStart: string;
  weekEnd: string;
  completed: number;
  target: number;
  percentage: number;
}

export interface MonthlyProgressDto {
  mode: DashboardMode;
  asOf: string;
  windowDays: number;
  buckets: MonthlyProgressBucketDto[];
  isEmpty: boolean;
}

@Injectable({ providedIn: 'root' })
export class MonthlyProgressService {
  private readonly _backend = inject(DashboardBackendService);

  get(asOf: string | null): Promise<MonthlyProgressDto> {
    return this._backend.get<MonthlyProgressDto>('api/v1.0/monthly-progress', {
      asOf: asOf ? new Date(asOf) : null
    });
  }
}
