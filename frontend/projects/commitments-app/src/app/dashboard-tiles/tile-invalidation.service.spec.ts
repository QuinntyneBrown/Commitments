import { Subject } from 'rxjs';

import { HubClient, RealtimeMessage } from '../core/hub-client';
import {
  DashboardTileDataInvalidatedPayload,
  TileInvalidationService
} from './tile-invalidation.service';

describe('TileInvalidationService', () => {
  let messages$: Subject<unknown>;
  let hubClient: HubClient;
  let service: TileInvalidationService;

  beforeEach(() => {
    messages$ = new Subject<unknown>();
    hubClient = { messages$, on: HubClient.prototype.on } as unknown as HubClient;
    service = new TileInvalidationService(hubClient);
  });

  function envelope(payload: DashboardTileDataInvalidatedPayload):
    RealtimeMessage<DashboardTileDataInvalidatedPayload> {
    return {
      schemaVersion: 1,
      messageId: '00000000-0000-0000-0000-000000000001',
      event: 'dashboardTileDataInvalidated',
      profileId: '11111111-2222-3333-4444-555555555555',
      occurredAt: new Date().toISOString(),
      correlationId: null,
      payload
    };
  }

  function payload(overrides: Partial<DashboardTileDataInvalidatedPayload> & {
    datasets: DashboardTileDataInvalidatedPayload['datasets']
  }): DashboardTileDataInvalidatedPayload {
    return {
      datasets: overrides.datasets,
      affectedGoalIds: overrides.affectedGoalIds ?? [],
      affectedCommitmentIds: overrides.affectedCommitmentIds ?? [],
      affectedToDoIds: overrides.affectedToDoIds ?? [],
      from: overrides.from ?? null,
      to: overrides.to ?? null,
      reason: overrides.reason ?? 'activityChanged'
    };
  }

  it('emits when invalidation includes the requested dataset', () => {
    const seen: DashboardTileDataInvalidatedPayload[] = [];
    service.invalidations$('dailyResults').subscribe(p => seen.push(p));

    messages$.next(envelope(payload({ datasets: ['dailyResults', 'weeklyFocus'] })));

    expect(seen).toHaveLength(1);
    expect(seen[0].datasets).toContain('dailyResults');
  });

  it('does not emit when invalidation does not include the requested dataset', () => {
    const seen: DashboardTileDataInvalidatedPayload[] = [];
    service.invalidations$('outstandingTodos').subscribe(p => seen.push(p));

    messages$.next(envelope(payload({ datasets: ['dailyResults'] })));

    expect(seen).toHaveLength(0);
  });

  it('routes the same envelope to multiple matching subscribers', () => {
    const dailyResults: DashboardTileDataInvalidatedPayload[] = [];
    const weeklyFocus: DashboardTileDataInvalidatedPayload[] = [];
    const outstandingTodos: DashboardTileDataInvalidatedPayload[] = [];
    service.invalidations$('dailyResults').subscribe(p => dailyResults.push(p));
    service.invalidations$('weeklyFocus').subscribe(p => weeklyFocus.push(p));
    service.invalidations$('outstandingTodos').subscribe(p => outstandingTodos.push(p));

    messages$.next(envelope(payload({ datasets: ['dailyResults', 'weeklyFocus'] })));

    expect(dailyResults).toHaveLength(1);
    expect(weeklyFocus).toHaveLength(1);
    expect(outstandingTodos).toHaveLength(0);
  });

  it('ignores non-matching event names (legacy or other envelopes)', () => {
    const seen: DashboardTileDataInvalidatedPayload[] = [];
    service.invalidations$('dailyResults').subscribe(p => seen.push(p));

    messages$.next({
      schemaVersion: 1,
      event: 'goalProgressUpdated',
      messageId: 'x',
      profileId: 'y',
      occurredAt: '2026-04-26T12:00:00Z',
      correlationId: null,
      payload: { datasets: ['dailyResults'] }
    });

    expect(seen).toHaveLength(0);
  });
});
