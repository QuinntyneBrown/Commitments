import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '../backend/dashboard-backend.service';
import { WindowFeatureBridgeService } from './window-feature-bridge.service';
import { provideMockDashboardFramework } from './provide-mock-dashboard-framework';

describe('provideMockDashboardFramework', () => {
  function makeInjector(backendResponses?: Record<string, unknown>) {
    return Injector.create({ providers: provideMockDashboardFramework({ backendResponses }) });
  }

  it('provides DashboardBackendService without HttpClient', () => {
    const injector = makeInjector();
    expect(() => injector.get(DashboardBackendService)).not.toThrow();
  });

  it('provides WindowFeatureBridgeService', () => {
    const injector = makeInjector();
    expect(() => injector.get(WindowFeatureBridgeService)).not.toThrow();
  });

  it('mock backend resolves from backendResponses on GET', async () => {
    const injector = makeInjector({ 'api/v1.0/profiles': { profiles: [{ id: '1' }] } });
    const backend = injector.get(DashboardBackendService);
    const result = await backend.get('api/v1.0/profiles');
    expect(result).toEqual({ profiles: [{ id: '1' }] });
  });

  it('mock backend records GET call on the bridge', async () => {
    const injector = makeInjector();
    const bridge = injector.get(WindowFeatureBridgeService);
    bridge.reset();
    const backend = injector.get(DashboardBackendService);
    await backend.get('api/v1.0/test');
    expect((window as any).__featureHarness.backendCalls).toEqual([
      { method: 'get', path: 'api/v1.0/test' }
    ]);
  });

  it('mock backend records POST call with body on the bridge', async () => {
    const injector = makeInjector();
    const bridge = injector.get(WindowFeatureBridgeService);
    bridge.reset();
    const backend = injector.get(DashboardBackendService);
    await backend.post('api/v1.0/users/token', { username: 'alice', password: 'pw' });
    expect((window as any).__featureHarness.backendCalls).toEqual([
      { method: 'post', path: 'api/v1.0/users/token', body: { username: 'alice', password: 'pw' } }
    ]);
  });
});
