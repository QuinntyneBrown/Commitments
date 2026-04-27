import { Subject } from 'rxjs';

import { HubClient, RealtimeMessage } from '../../core/hub-client';
import { GoalProgress, GoalProgressService, Last14DayPoint } from '../../services/goal-progress.service';

import { GoalProgressUpdatedPayload, LiveGoalMetricsController } from './live-goal-metrics-controller';

describe('LiveGoalMetricsController', () => {
  let messages$: Subject<unknown>;
  let hubClient: HubClient;
  let goalProgressService: GoalProgressService;
  let getCurrent: jest.Mock;
  let getLast14: jest.Mock;

  beforeEach(() => {
    messages$ = new Subject<unknown>();
    hubClient = { messages$, on: HubClient.prototype.on } as unknown as HubClient;
    getCurrent = jest.fn();
    getLast14 = jest.fn().mockReturnValue({ subscribe: (fn: (v: Last14DayPoint[]) => void) => fn([]) });
    goalProgressService = { getCurrent, getLast14 } as unknown as GoalProgressService;
  });

  function payload(overrides: Partial<GoalProgressUpdatedPayload> & { goalId: string; count: number }): GoalProgressUpdatedPayload {
    return {
      goalId: overrides.goalId,
      behaviourId: null,
      count: overrides.count,
      target: overrides.target ?? 30,
      percent: overrides.percent ?? 0,
      asOf: overrides.asOf ?? new Date().toISOString(),
      date: overrides.date ?? new Date().toISOString().slice(0, 10),
      reason: overrides.reason ?? 'activityCreated',
      sourceActivityId: overrides.sourceActivityId ?? null
    };
  }

  function envelope(p: GoalProgressUpdatedPayload): RealtimeMessage<GoalProgressUpdatedPayload> {
    return {
      schemaVersion: 1,
      messageId: '00000000-0000-0000-0000-000000000001',
      event: 'goalProgressUpdated',
      profileId: '11111111-2222-3333-4444-555555555555',
      occurredAt: new Date().toISOString(),
      correlationId: null,
      payload: p
    };
  }

  it('exposes initial values from the GoalProgressService', () => {
    const initial: GoalProgress = {
      goalId: 'goal-1',
      target: 30,
      count: 5,
      asOf: new Date('2026-04-26T12:00:00Z')
    };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = new LiveGoalMetricsController(hubClient, goalProgressService);
    controller.load('goal-1');

    expect(controller.count()).toBe(5);
    expect(controller.target()).toBe(30);
    expect(controller.pct()).toBeCloseTo((5 / 30) * 100);
  });

  it('updates count when a goalProgressUpdated message arrives for the same goal', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 5, asOf: new Date() };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = new LiveGoalMetricsController(hubClient, goalProgressService);
    controller.load('goal-1');

    messages$.next(envelope(payload({ goalId: 'goal-1', count: 12 })));

    expect(controller.count()).toBe(12);
  });

  it('ignores goalProgressUpdated messages for a different goal', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 5, asOf: new Date() };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = new LiveGoalMetricsController(hubClient, goalProgressService);
    controller.load('goal-1');

    messages$.next(envelope(payload({ goalId: 'goal-2', count: 99 })));

    expect(controller.count()).toBe(5);
  });

  it('clamps pct to 0..100 even when count exceeds target', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 10, count: 25, asOf: new Date() };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = new LiveGoalMetricsController(hubClient, goalProgressService);
    controller.load('goal-1');

    expect(controller.pct()).toBe(100);
  });

  it('loads the last14 series and exposes deltaVsYesterday', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 5, asOf: new Date() };
    const last14: Last14DayPoint[] = Array.from({ length: 14 }, (_, i) => ({
      date: `2026-04-${String(13 + i).padStart(2, '0')}`,
      completed: i,
      target: 30,
      percent: Math.round((i / 30) * 100)
    }));
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });
    getLast14.mockReturnValue({ subscribe: (fn: (v: Last14DayPoint[]) => void) => fn(last14) });

    const controller = new LiveGoalMetricsController(hubClient, goalProgressService);
    controller.load('goal-1');

    expect(controller.last14().length).toBe(14);
    expect(controller.deltaVsYesterday()).toBe(1);
  });

  it('patches todays last14 entry when a goalProgressUpdated message arrives', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 5, asOf: new Date() };
    const last14: Last14DayPoint[] = Array.from({ length: 14 }, (_, i) => ({
      date: `2026-04-${String(13 + i).padStart(2, '0')}`,
      completed: i,
      target: 30,
      percent: Math.round((i / 30) * 100)
    }));
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });
    getLast14.mockReturnValue({ subscribe: (fn: (v: Last14DayPoint[]) => void) => fn(last14) });

    const controller = new LiveGoalMetricsController(hubClient, goalProgressService);
    controller.load('goal-1');

    messages$.next(envelope(payload({ goalId: 'goal-1', count: 22 })));

    const todayPoint = controller.last14()[13];
    expect(todayPoint.completed).toBe(22);
  });
});
