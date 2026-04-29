import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { CardService } from './card.service';

describe('CardService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new CardService()
    );
  }

  it('GETs the card list', async () => {
    const get = jest.fn().mockResolvedValue({ cards: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/cards');
  });

  it('POSTs to save a card', async () => {
    const post = jest.fn().mockResolvedValue({ card: {} });
    await make({ post }).save({ name: 'My Card' });
    expect(post).toHaveBeenCalledWith('api/v1.0/cards', { name: 'My Card' });
  });

  it('DELETEs a card by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(1);
    expect(del).toHaveBeenCalledWith('api/v1.0/cards/1');
  });
});
