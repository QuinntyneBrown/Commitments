import { TestBed } from '@angular/core/testing';
import { Subject } from 'rxjs';

import { HubClient } from '../../core/hub-client';
import { GoalProgress, GoalProgressService } from '../../services/goal-progress.service';

import { LiveGoalMetricsController } from './live-goal-metrics-controller';

describe('LiveGoalMetricsController', () => {
  let messages$: Subject<unknown>;
  let getCurrent: jest.Mock;

  beforeEach(() => {
    messages$ = new Subject<unknown>();
    getCurrent = jest.fn();

    TestBed.configureTestingModule({
      providers: [
        LiveGoalMetricsController,
        { provide: HubClient, useValue: { messages$, connect: () => Promise.resolve() } },
        { provide: GoalProgressService, useValue: { getCurrent } as Partial<GoalProgressService> }
      ]
    });
  });

  it('exposes initial values from the GoalProgressService', () => {
    const initial: GoalProgress = {
      goalId: 'goal-1',
      target: 30,
      count: 5,
      asOf: new Date('2026-04-26T12:00:00Z')
    };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = TestBed.inject(LiveGoalMetricsController);
    controller.load('goal-1');

    expect(controller.count()).toBe(5);
    expect(controller.target()).toBe(30);
    expect(controller.pct()).toBeCloseTo((5 / 30) * 100);
  });

  it('updates count when a goalProgressUpdated message arrives for the same goal', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 5, asOf: new Date() };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = TestBed.inject(LiveGoalMetricsController);
    controller.load('goal-1');

    messages$.next({ event: 'goalProgressUpdated', goalId: 'goal-1', count: 12, asOf: new Date().toISOString() });

    expect(controller.count()).toBe(12);
  });

  it('ignores goalProgressUpdated messages for a different goal', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 5, asOf: new Date() };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = TestBed.inject(LiveGoalMetricsController);
    controller.load('goal-1');

    messages$.next({ event: 'goalProgressUpdated', goalId: 'goal-2', count: 99, asOf: new Date().toISOString() });

    expect(controller.count()).toBe(5);
  });

  it('clamps pct to 0..100 even when count exceeds target', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 10, count: 25, asOf: new Date() };
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = TestBed.inject(LiveGoalMetricsController);
    controller.load('goal-1');

    expect(controller.pct()).toBe(100);
  });
});
