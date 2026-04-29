import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { FrequencyType } from './frequency-type';

@Injectable({ providedIn: 'root' })
export class FrequencyTypeService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ frequencyTypes: FrequencyType[] }> { return this._backend.get('api/v1.0/frequencyTypes'); }
}
