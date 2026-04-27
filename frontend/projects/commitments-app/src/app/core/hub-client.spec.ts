import { Subject } from 'rxjs';

import { HubClient, RealtimeMessage } from './hub-client';

describe('HubClient.on<T>(event)', () => {
  let messages$: Subject<unknown>;
  let hub: HubClient;

  beforeEach(() => {
    messages$ = new Subject<unknown>();
    hub = { messages$, on: HubClient.prototype.on } as unknown as HubClient;
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

  it('emits payload when event matches and schemaVersion is 1', () => {
    const seen: Array<{ text: string }> = [];
    hub.on<{ text: string }>('hubPing').subscribe(p => seen.push(p));

    messages$.next(envelope('hubPing', { text: 'hello' }));

    expect(seen).toEqual([{ text: 'hello' }]);
  });

  it('does not emit when event name differs', () => {
    const seen: Array<{ text: string }> = [];
    hub.on<{ text: string }>('hubPing').subscribe(p => seen.push(p));

    messages$.next(envelope('goalProgressUpdated', { text: 'nope' }));

    expect(seen).toHaveLength(0);
  });

  it('does not emit non-envelope messages (legacy flat shape)', () => {
    const seen: Array<unknown> = [];
    hub.on<unknown>('hubPing').subscribe(p => seen.push(p));

    messages$.next({ event: 'hubPing', text: 'legacy-flat' });

    expect(seen).toHaveLength(0);
  });

  it('does not emit when schemaVersion is not 1', () => {
    const seen: Array<unknown> = [];
    hub.on<unknown>('hubPing').subscribe(p => seen.push(p));

    messages$.next({ ...envelope('hubPing', { text: 'v2' }), schemaVersion: 2 } as unknown);

    expect(seen).toHaveLength(0);
  });

  it('emits multiple payloads in order for the same event', () => {
    const seen: number[] = [];
    hub.on<{ n: number }>('tick').subscribe(p => seen.push(p.n));

    messages$.next(envelope('tick', { n: 1 }));
    messages$.next(envelope('tick', { n: 2 }));
    messages$.next(envelope('tick', { n: 3 }));

    expect(seen).toEqual([1, 2, 3]);
  });
});
