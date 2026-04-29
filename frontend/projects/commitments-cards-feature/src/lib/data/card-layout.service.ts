import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { CardLayout } from './card-layout';

@Injectable({ providedIn: 'root' })
export class CardLayoutService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ cardLayouts: CardLayout[] }>              { return this._backend.get('api/v1.0/cardLayouts'); }
  save(input: Partial<CardLayout>): Promise<{ cardLayout: CardLayout }> { return this._backend.post('api/v1.0/cardLayouts', input); }
  remove(id: number | string): Promise<void>                 { return this._backend.delete(`api/v1.0/cardLayouts/${id}`); }
}
