import { HttpClient, HttpParams } from '@angular/common/http';
import { Injector, runInInjectionContext } from '@angular/core';
import { of } from 'rxjs';

import { DASHBOARD_BACKEND_BASE_URL, DashboardBackendService } from './dashboard-backend.service';

describe('DashboardBackendService', () => {
  it('owns backend GET transport for dashboard plugins', async () => {
    const get = jest.fn().mockReturnValue(of({ ok: true }));
    const injector = Injector.create({
      providers: [
        { provide: HttpClient, useValue: { get } },
        { provide: DASHBOARD_BACKEND_BASE_URL, useValue: 'http://api.example/' }
      ]
    });
    const service = runInInjectionContext(injector, () => new DashboardBackendService());

    const response = await service.get<{ ok: boolean }>('api/v1.0/example', {
      asOf: new Date('2026-04-15T18:30:00Z'),
      includeToday: true,
      windowDays: 30,
      omitted: null
    });

    expect(get).toHaveBeenCalledWith('http://api.example/api/v1.0/example', {
      params: expect.any(HttpParams)
    });
    const params = get.mock.calls[0][1].params as HttpParams;
    expect(params.get('asOf')).toBe('2026-04-15T18:30:00.000Z');
    expect(params.get('includeToday')).toBe('true');
    expect(params.get('windowDays')).toBe('30');
    expect(params.has('omitted')).toBe(false);
    expect(response).toEqual({ ok: true });
  });
});
