import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { Behaviour } from './behaviour';

@Injectable({ providedIn: 'root' })
export class BehaviourService {
  private readonly _backend = inject(DashboardBackendService);

  list(): Promise<{ behaviours: Behaviour[] }> { return this._backend.get('api/v1.0/behaviours'); }
  create(input: Partial<Behaviour>): Promise<{ behaviour: Behaviour }> { return this._backend.post('api/v1.0/behaviours', input); }
  update(input: Partial<Behaviour>): Promise<{ behaviour: Behaviour }> { return this._backend.put('api/v1.0/behaviours', input); }
  remove(id: number | string): Promise<void> { return this._backend.delete(`api/v1.0/behaviours/${id}`); }
}
