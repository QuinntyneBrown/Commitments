import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { DASHBOARD_BACKEND_BASE_URL, DashboardBackendService } from './dashboard-backend.service';

describe('DashboardBackendService', () => {
  let service: DashboardBackendService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: DASHBOARD_BACKEND_BASE_URL, useValue: 'http://api.example/' }]
    });

    service = TestBed.inject(DashboardBackendService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('owns backend GET transport for dashboard plugins', async () => {
    const response = service.get<{ ok: boolean }>('api/v1.0/example', {
      asOf: new Date('2026-04-15T18:30:00Z'),
      includeToday: true,
      windowDays: 30,
      omitted: null
    });

    const req = http.expectOne((request) => request.url === 'http://api.example/api/v1.0/example');
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('asOf')).toBe('2026-04-15T18:30:00.000Z');
    expect(req.request.params.get('includeToday')).toBe('true');
    expect(req.request.params.get('windowDays')).toBe('30');
    expect(req.request.params.has('omitted')).toBe(false);

    req.flush({ ok: true });

    await expect(response).resolves.toEqual({ ok: true });
  });
});
