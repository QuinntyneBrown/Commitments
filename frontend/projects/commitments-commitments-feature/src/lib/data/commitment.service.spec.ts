import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { CommitmentService } from './commitment.service';

describe('CommitmentService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new CommitmentService()
    );
  }

  it('GETs the commitment list', async () => {
    const get = jest.fn().mockResolvedValue({ commitments: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/commitments');
  });

  it('POSTs to save a commitment', async () => {
    const post = jest.fn().mockResolvedValue({ commitment: {} });
    await make({ post }).save({ behaviourId: 1 });
    expect(post).toHaveBeenCalledWith('api/v1.0/commitments', { behaviourId: 1 });
  });

  it('DELETEs a commitment by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(42);
    expect(del).toHaveBeenCalledWith('api/v1.0/commitments/42');
  });
});
