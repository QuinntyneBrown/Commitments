import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { BehaviourType } from './behaviour-type';

@Injectable({ providedIn: 'root' })
export class BehaviourTypeService {
  private readonly _backend = inject(DashboardBackendService);

  list(): Promise<{ behaviourTypes: BehaviourType[] }> { return this._backend.get('api/v1.0/behaviourTypes'); }
  create(input: Partial<BehaviourType>): Promise<{ behaviourType: BehaviourType }> { return this._backend.post('api/v1.0/behaviourTypes', input); }
  update(input: Partial<BehaviourType>): Promise<{ behaviourType: BehaviourType }> { return this._backend.put('api/v1.0/behaviourTypes', input); }
  remove(id: number | string): Promise<void> { return this._backend.delete(`api/v1.0/behaviourTypes/${id}`); }
}
