import { Subject } from 'rxjs';

import { HubClient, RealtimeMessage } from './hub-client';
import { Store } from './store';

describe('Store realtime getters (slice 18)', () => {
  let messages$: Subject<unknown>;
  let hubClient: HubClient;
  let store: Store;

  beforeEach(() => {
    messages$ = new Subject<unknown>();
    hubClient = { messages$, on: HubClient.prototype.on } as unknown as HubClient;
    store = new Store(hubClient);
  });

  function envelope<T>(event: string, payload: T): RealtimeMessage<T> {
    return {
      schemaVersion: 1,
      messageId: '00000000-0000-0000-0000-000000000001',
      event,
      profileId: '11111111-2222-3333-4444-555555555555',
      occurredAt: '2026-04-26T12:00:00Z',
      correlationId: null,
      payload
    };
  }

  it('savedNotes$ wraps noteSaved payload as { note: { noteId, title } }', () => {
    const seen: any[] = [];
    store.savedNotes$.subscribe(v => seen.push(v));

    messages$.next(envelope('noteSaved', {
      noteId: 'n-1',
      title: 'Weekly review',
      kind: 'Updated'
    }));

    expect(seen).toEqual([{ note: { noteId: 'n-1', title: 'Weekly review' } }]);
  });

  it('removedNotes$ emits { noteId } for noteRemoved', () => {
    const seen: any[] = [];
    store.removedNotes$.subscribe(v => seen.push(v));

    messages$.next(envelope('noteRemoved', { noteId: 'n-2' }));

    expect(seen).toEqual([{ noteId: 'n-2' }]);
  });

  it('savedTags$ wraps tagSaved payload as { tag: { tagId, name } }', () => {
    const seen: any[] = [];
    store.savedTags$.subscribe(v => seen.push(v));

    messages$.next(envelope('tagSaved', { tagId: 't-1', name: 'weekly' }));

    expect(seen).toEqual([{ tag: { tagId: 't-1', name: 'weekly' } }]);
  });

  it('removedTags$ emits { tagId } for tagRemoved', () => {
    const seen: any[] = [];
    store.removedTags$.subscribe(v => seen.push(v));

    messages$.next(envelope('tagRemoved', { tagId: 't-2' }));

    expect(seen).toEqual([{ tagId: 't-2' }]);
  });

  it('legacy [Note] Saved messages no longer emit on savedNotes$', () => {
    const seen: any[] = [];
    store.savedNotes$.subscribe(v => seen.push(v));

    messages$.next({ type: '[Note] Saved', payload: { note: { noteId: 'old' } } });

    expect(seen).toHaveLength(0);
  });
});
