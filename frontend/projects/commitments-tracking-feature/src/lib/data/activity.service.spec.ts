import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { ActivityService } from './activity.service';

describe('ActivityService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new ActivityService()
    );
  }

  it('GETs the activity list', async () => {
    const get = jest.fn().mockResolvedValue({ activities: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/activities');
  });

  it('POSTs to record an activity', async () => {
    const post = jest.fn().mockResolvedValue({ activity: {} });
    await make({ post }).record({ commitmentId: 1, performedOn: '2026-04-29T00:00:00.000Z' });
    expect(post).toHaveBeenCalledWith('api/v1.0/activities', { commitmentId: 1, performedOn: '2026-04-29T00:00:00.000Z' });
  });

  it('DELETEs an activity by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(42);
    expect(del).toHaveBeenCalledWith('api/v1.0/activities/42');
  });
});
