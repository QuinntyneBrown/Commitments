import { TestBed } from '@angular/core/testing';
import { DASHBOARD_BACKEND_BASE_URL } from '@commitments/dashboard-framework';
import { appConfig } from './app.config';

describe('appConfig', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ providers: appConfig.providers });
  });

  it('DASHBOARD_BACKEND_BASE_URL points to the running backend HTTP port (bug-193)', () => {
    const url = TestBed.inject(DASHBOARD_BACKEND_BASE_URL);
    expect(url).toBe('http://localhost:63714/');
  });
});
