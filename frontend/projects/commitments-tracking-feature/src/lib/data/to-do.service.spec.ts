import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { ToDoService } from './to-do.service';

describe('ToDoService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new ToDoService()
    );
  }

  it('GETs the to-do list', async () => {
    const get = jest.fn().mockResolvedValue({ toDos: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/toDos');
  });

  it('POSTs to create a to-do', async () => {
    const post = jest.fn().mockResolvedValue({ toDo: {} });
    await make({ post }).create({ commitmentId: 1, dueOn: '2026-04-29T00:00:00.000Z' });
    expect(post).toHaveBeenCalledWith('api/v1.0/toDos', { commitmentId: 1, dueOn: '2026-04-29T00:00:00.000Z' });
  });

  it('DELETEs a to-do by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(42);
    expect(del).toHaveBeenCalledWith('api/v1.0/toDos/42');
  });
});
