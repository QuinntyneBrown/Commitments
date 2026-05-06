import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '../backend';
import { Commitment } from './commitment';

@Injectable({ providedIn: 'root' })
export class CommitmentService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ commitments: Commitment[] }>   { return this._backend.get('api/v1.0/commitments'); }
  save(input: Partial<Commitment>): Promise<{ commitment: Commitment }> { return this._backend.post('api/v1.0/commitments', input); }
  remove(id: number | string): Promise<void>       { return this._backend.delete(`api/v1.0/commitments/${id}`); }
}
