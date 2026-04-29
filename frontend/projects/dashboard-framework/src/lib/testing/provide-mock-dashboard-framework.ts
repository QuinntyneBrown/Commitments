import { Provider } from '@angular/core';
import { DASHBOARD_BACKEND_BASE_URL, DashboardBackendService } from '../backend/dashboard-backend.service';
import { BackendCall, WindowFeatureBridgeService } from './window-feature-bridge.service';

export interface MockDashboardFrameworkOptions {
  backendResponses?: Record<string, unknown>;
}

function createMockBackend(
  bridge: WindowFeatureBridgeService,
  responses: Record<string, unknown>
): DashboardBackendService {
  return {
    get(path: string, params?: Record<string, unknown>) {
      const call: BackendCall = { method: 'get', path };
      if (params) call.params = params;
      bridge.recordBackend(call);
      return Promise.resolve(responses[path] ?? {});
    },
    post(path: string, body: unknown) {
      bridge.recordBackend({ method: 'post', path, body });
      return Promise.resolve(responses[path] ?? {});
    },
    put(path: string, body: unknown) {
      bridge.recordBackend({ method: 'put', path, body });
      return Promise.resolve(responses[path] ?? {});
    },
    delete(path: string) {
      bridge.recordBackend({ method: 'delete', path });
      return Promise.resolve(responses[path] ?? undefined);
    }
  } as unknown as DashboardBackendService;
}

export function provideMockDashboardFramework(opts: MockDashboardFrameworkOptions = {}): Provider[] {
  const responses = opts.backendResponses ?? {};
  return [
    WindowFeatureBridgeService,
    { provide: DASHBOARD_BACKEND_BASE_URL, useValue: '' },
    {
      provide: DashboardBackendService,
      useFactory: (bridge: WindowFeatureBridgeService) => createMockBackend(bridge, responses),
      deps: [WindowFeatureBridgeService]
    }
  ];
}
