import { WindowFeatureBridgeService } from './window-feature-bridge.service';

describe('WindowFeatureBridgeService', () => {
  let service: WindowFeatureBridgeService;

  beforeEach(() => {
    service = new WindowFeatureBridgeService();
  });

  it('initializes window.__featureHarness on construction', () => {
    expect((window as any).__featureHarness).toBeDefined();
    expect((window as any).__featureHarness.feature).toBeNull();
    expect((window as any).__featureHarness.backendCalls).toEqual([]);
  });

  it('setFeature updates the snapshot feature field', () => {
    service.setFeature('identity');
    expect((window as any).__featureHarness.feature).toBe('identity');
  });

  it('setRoute updates the snapshot route field', () => {
    service.setRoute('/login');
    expect((window as any).__featureHarness.route).toBe('/login');
  });

  it('recordBackend pushes a call into backendCalls', () => {
    service.recordBackend({ method: 'get', path: 'api/v1.0/profiles' });
    const calls = (window as any).__featureHarness.backendCalls;
    expect(calls).toHaveLength(1);
    expect(calls[0]).toEqual({ method: 'get', path: 'api/v1.0/profiles' });
  });

  it('reset clears all snapshot fields to initial state', () => {
    service.setFeature('identity');
    service.setRoute('/login');
    service.recordBackend({ method: 'post', path: 'api/v1.0/users/token', body: {} });
    service.reset();
    const snap = (window as any).__featureHarness;
    expect(snap.feature).toBeNull();
    expect(snap.route).toBeNull();
    expect(snap.backendCalls).toHaveLength(0);
  });
});
