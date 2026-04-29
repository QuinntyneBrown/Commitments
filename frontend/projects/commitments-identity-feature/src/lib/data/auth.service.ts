import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _backend = inject(DashboardBackendService);

  token(input: { username: string; password: string }): Promise<{ accessToken: string; profileId: string }> {
    return this._backend.post('api/v1.0/users/token', input);
  }
}
