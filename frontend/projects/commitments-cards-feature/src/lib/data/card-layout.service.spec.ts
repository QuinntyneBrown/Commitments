import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { CardLayoutService } from './card-layout.service';

describe('CardLayoutService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new CardLayoutService()
    );
  }

  it('GETs the card layouts list', async () => {
    const get = jest.fn().mockResolvedValue({ cardLayouts: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/cardLayouts');
  });

  it('POSTs to save a card layout', async () => {
    const post = jest.fn().mockResolvedValue({ cardLayout: {} });
    await make({ post }).save({ name: 'Layout 1' });
    expect(post).toHaveBeenCalledWith('api/v1.0/cardLayouts', { name: 'Layout 1' });
  });

  it('DELETEs a card layout by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(1);
    expect(del).toHaveBeenCalledWith('api/v1.0/cardLayouts/1');
  });
});
