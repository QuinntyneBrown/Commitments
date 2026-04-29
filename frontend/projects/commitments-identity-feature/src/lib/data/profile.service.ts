import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { Profile } from './profile';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private readonly _backend = inject(DashboardBackendService);

  list(): Promise<{ profiles: Profile[] }> {
    return this._backend.get('api/v1.0/profiles');
  }

  current(): Promise<{ profile: Profile }> {
    return this._backend.get('api/v1.0/profiles/current');
  }
}
