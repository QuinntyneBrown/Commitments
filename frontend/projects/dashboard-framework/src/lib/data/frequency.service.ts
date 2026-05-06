import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { Frequency } from './frequency';

@Injectable({ providedIn: 'root' })
export class FrequencyService {
  private readonly _backend = inject(DashboardBackendService);

  list(): Promise<{ frequencies: Frequency[] }> { return this._backend.get('api/v1.0/frequencies'); }
  getById(id: number | string): Promise<{ frequency: Frequency }> { return this._backend.get(`api/v1.0/frequencies/${id}`); }
  save(input: Partial<Frequency>): Promise<{ frequency: Frequency }> { return this._backend.post('api/v1.0/frequencies', input); }
  remove(id: number | string): Promise<void> { return this._backend.delete(`api/v1.0/frequencies/${id}`); }
}
