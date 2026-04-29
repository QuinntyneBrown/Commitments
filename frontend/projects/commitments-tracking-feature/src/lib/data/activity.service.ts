import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { Activity } from './activity';

@Injectable({ providedIn: 'root' })
export class ActivityService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ activities: Activity[] }>         { return this._backend.get('api/v1.0/activities'); }
  record(input: Partial<Activity>): Promise<{ activity: Activity }> { return this._backend.post('api/v1.0/activities', input); }
  remove(id: number | string): Promise<void>          { return this._backend.delete(`api/v1.0/activities/${id}`); }
}
