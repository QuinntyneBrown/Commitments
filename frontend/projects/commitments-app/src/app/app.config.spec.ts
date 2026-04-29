import { DASHBOARD_BACKEND_BASE_URL } from '@commitments/dashboard-framework';
import { appConfig } from './app.config';

describe('appConfig', () => {
  it('DASHBOARD_BACKEND_BASE_URL points to the running backend HTTP port (bug-193)', () => {
    const provider = appConfig.providers.find(
      (p): p is { provide: unknown; useValue: string } =>
        typeof p === 'object' && p !== null && 'provide' in p && (p as { provide: unknown }).provide === DASHBOARD_BACKEND_BASE_URL
    );
    expect(provider).toBeDefined();
    expect(provider!.useValue).toBe('http://localhost:63714/');
  });
});
