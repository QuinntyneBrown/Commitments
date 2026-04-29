import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { BehaviourService } from './behaviour.service';

describe('BehaviourService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new BehaviourService()
    );
  }

  it('GETs behaviour list', async () => {
    const get = jest.fn().mockResolvedValue({ behaviours: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/behaviours');
  });

  it('POSTs to create a behaviour', async () => {
    const post = jest.fn().mockResolvedValue({ behaviourId: 1 });
    await make({ post }).create({ name: 'Test', behaviourTypeId: 1 });
    expect(post).toHaveBeenCalledWith('api/v1.0/behaviours', { name: 'Test', behaviourTypeId: 1 });
  });

  it('PUTs to update a behaviour', async () => {
    const put = jest.fn().mockResolvedValue({});
    await make({ put }).update({ behaviourId: 1, name: 'Updated', behaviourTypeId: 1 });
    expect(put).toHaveBeenCalledWith('api/v1.0/behaviours', { behaviourId: 1, name: 'Updated', behaviourTypeId: 1 });
  });

  it('DELETEs a behaviour by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove('abc');
    expect(del).toHaveBeenCalledWith('api/v1.0/behaviours/abc');
  });
});
