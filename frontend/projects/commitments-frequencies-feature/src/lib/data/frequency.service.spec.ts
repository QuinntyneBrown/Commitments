import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { FrequencyService } from './frequency.service';

describe('FrequencyService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new FrequencyService()
    );
  }

  it('GETs the frequency list', async () => {
    const get = jest.fn().mockResolvedValue({ frequencies: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/frequencies');
  });

  it('GETs a single frequency by id', async () => {
    const get = jest.fn().mockResolvedValue({ frequency: {} });
    await make({ get }).getById(42);
    expect(get).toHaveBeenCalledWith('api/v1.0/frequencies/42');
  });

  it('POSTs to save a frequency', async () => {
    const post = jest.fn().mockResolvedValue({ frequency: {} });
    await make({ post }).save({ frequency: 3, frequencyTypeId: 1 });
    expect(post).toHaveBeenCalledWith('api/v1.0/frequencies', { frequency: 3, frequencyTypeId: 1 });
  });

  it('DELETEs a frequency by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(42);
    expect(del).toHaveBeenCalledWith('api/v1.0/frequencies/42');
  });
});
