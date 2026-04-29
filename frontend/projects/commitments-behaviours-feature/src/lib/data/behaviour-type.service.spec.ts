import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { BehaviourTypeService } from './behaviour-type.service';

describe('BehaviourTypeService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new BehaviourTypeService()
    );
  }

  it('GETs behaviour type list', async () => {
    const get = jest.fn().mockResolvedValue({ behaviourTypes: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/behaviourTypes');
  });

  it('POSTs to create a behaviour type', async () => {
    const post = jest.fn().mockResolvedValue({ behaviourTypeId: 1 });
    await make({ post }).create({ name: 'Productive' });
    expect(post).toHaveBeenCalledWith('api/v1.0/behaviourTypes', { name: 'Productive' });
  });

  it('PUTs to update a behaviour type', async () => {
    const put = jest.fn().mockResolvedValue({});
    await make({ put }).update({ behaviourTypeId: 1, name: 'Updated' });
    expect(put).toHaveBeenCalledWith('api/v1.0/behaviourTypes', { behaviourTypeId: 1, name: 'Updated' });
  });

  it('DELETEs a behaviour type by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove('abc');
    expect(del).toHaveBeenCalledWith('api/v1.0/behaviourTypes/abc');
  });
});
