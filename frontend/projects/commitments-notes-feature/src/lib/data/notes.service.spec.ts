import { Injector, runInInjectionContext } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { NotesService } from './notes.service';

describe('NotesService', () => {
  function make(mock: Partial<DashboardBackendService>) {
    return runInInjectionContext(
      Injector.create({ providers: [{ provide: DashboardBackendService, useValue: mock }] }),
      () => new NotesService()
    );
  }

  it('GETs the notes list', async () => {
    const get = jest.fn().mockResolvedValue({ notes: [] });
    await make({ get }).list();
    expect(get).toHaveBeenCalledWith('api/v1.0/notes');
  });

  it('GETs a note by slug', async () => {
    const get = jest.fn().mockResolvedValue({ note: {} });
    await make({ get }).getBySlug('getting-started');
    expect(get).toHaveBeenCalledWith('api/v1.0/notes/slug/getting-started');
  });

  it('GETs notes by tag slug', async () => {
    const get = jest.fn().mockResolvedValue({ notes: [] });
    await make({ get }).getByTagSlug('work');
    expect(get).toHaveBeenCalledWith('api/v1.0/notes/tag/work');
  });

  it('POSTs to save a note', async () => {
    const post = jest.fn().mockResolvedValue({ note: {} });
    const note = { noteId: 1, slug: 'test', title: 'Test', body: '<p>test</p>' };
    await make({ post }).save({ note });
    expect(post).toHaveBeenCalledWith('api/v1.0/notes', { note });
  });

  it('DELETEs a note by id', async () => {
    const del = jest.fn().mockResolvedValue(undefined);
    await make({ delete: del }).remove(1);
    expect(del).toHaveBeenCalledWith('api/v1.0/notes/1');
  });
});
