import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { Card } from './card';

@Injectable({ providedIn: 'root' })
export class CardService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ cards: Card[] }>               { return this._backend.get('api/v1.0/cards'); }
  save(input: Partial<Card>): Promise<{ card: Card }> { return this._backend.post('api/v1.0/cards', input); }
  remove(id: number | string): Promise<void>       { return this._backend.delete(`api/v1.0/cards/${id}`); }
}
