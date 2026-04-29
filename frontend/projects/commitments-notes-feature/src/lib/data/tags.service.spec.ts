import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { TagsService } from './tags.service';

describe('TagsService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new TagsService()
    );
  }

  it('GETs the tags list', async () => {
    const get = jest.fn().mockResolvedValue({ tags: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/tags');
  });

  it('POSTs to save a tag', async () => {
    const post = jest.fn().mockResolvedValue({ tag: {} });
    await make({ post }).save({ name: 'work', slug: 'work' });
    expect(post).toHaveBeenCalledWith('api/v1.0/tags', { name: 'work', slug: 'work' });
  });

  it('DELETEs a tag by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(1);
    expect(del).toHaveBeenCalledWith('api/v1.0/tags/1');
  });
});
