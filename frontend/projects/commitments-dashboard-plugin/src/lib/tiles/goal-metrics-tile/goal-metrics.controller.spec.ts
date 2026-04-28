import { of } from 'rxjs';

import { GoalMetricsController } from './goal-metrics.controller';
import { GoalProgressDto, GoalProgressService } from '../../data/goal-progress.service';

function dto(overrides: Partial<GoalProgressDto> = {}): GoalProgressDto {
  return {
    goalId: 'g1',
    count: 10,
    target: 30,
    asOf: '2026-04-27T17:00:00Z',
    ...overrides
  };
}

describe('GoalMetricsController', () => {
  let getCurrent: jest.Mock;
  let getAt: jest.Mock;
  let service: GoalProgressService;

  beforeEach(() => {
    getCurrent = jest.fn();
    getAt = jest.fn();
    service = { getCurrent, getAt } as unknown as GoalProgressService;
  });

  it('starts with zero count/target/percentage and no delta', () => {
    const controller = new GoalMetricsController(service);

    expect(controller.count()).toBe(0);
    expect(controller.target()).toBe(0);
    expect(controller.percentage()).toBe(0);
    expect(controller.deltaLabel()).toBeNull();
  });

  it('does nothing when goalId is unset', () => {
    const controller = new GoalMetricsController(service);

    controller.load('live', null);

    expect(getCurrent).not.toHaveBeenCalled();
  });

  it('load("live") fetches via getCurrent and populates current', () => {
    getCurrent.mockReturnValue(of(dto({ count: 12, target: 30 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('live', null);

    expect(getCurrent).toHaveBeenCalledWith('g1');
    expect(controller.count()).toBe(12);
    expect(controller.target()).toBe(30);
    expect(controller.percentage()).toBe(40);
  });

  it('load("review") fetches both via getAt + getCurrent (parallel)', () => {
    getAt.mockReturnValue(of(dto({ count: 5, target: 30, asOf: '2026-04-15T18:30:00Z' })));
    getCurrent.mockReturnValue(of(dto({ count: 12, target: 30 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(getAt).toHaveBeenCalledWith('g1', '2026-04-15T18:30:00Z');
    expect(getCurrent).toHaveBeenCalledWith('g1');
    expect(controller.count()).toBe(5);
  });

  it('clears today when switching from review to live', () => {
    getAt.mockReturnValue(of(dto({ count: 5 })));
    getCurrent.mockReturnValue(of(dto({ count: 12 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('review', '2026-04-15T18:30:00Z');
    expect(controller.deltaLabel()).not.toBeNull();

    controller.load('live', null);

    expect(controller.deltaLabel()).toBeNull();
  });

  it('applyHubUpdate updates current count/asOf in live mode for matching goal', () => {
    getCurrent.mockReturnValue(of(dto({ count: 10 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('live', null);

    controller.applyHubUpdate({ goalId: 'g1', count: 15, asOf: '2026-04-27T18:00:00Z' });

    expect(controller.count()).toBe(15);
  });

  it('ignores hub update in review mode', () => {
    getAt.mockReturnValue(of(dto({ count: 5 })));
    getCurrent.mockReturnValue(of(dto({ count: 10 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('review', '2026-04-15T18:30:00Z');

    controller.applyHubUpdate({ goalId: 'g1', count: 99, asOf: '2026-04-27T18:00:00Z' });

    expect(controller.count()).toBe(5); // unchanged
  });

  it('ignores hub update for a different goalId', () => {
    getCurrent.mockReturnValue(of(dto({ goalId: 'g1', count: 10 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('live', null);

    controller.applyHubUpdate({ goalId: 'other', count: 99, asOf: '2026-04-27T18:00:00Z' });

    expect(controller.count()).toBe(10);
  });

  it('formats positive delta as "+N vs today" with success tone', () => {
    // historical < today → today is better → success
    getAt.mockReturnValue(of(dto({ count: 5 })));
    getCurrent.mockReturnValue(of(dto({ count: 12 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBe('-7 vs today');
    expect(controller.deltaTone()).toBe('success');
  });

  it('formats positive delta when historical > today with warn tone', () => {
    // historical > today → today is worse → warn
    getAt.mockReturnValue(of(dto({ count: 12 })));
    getCurrent.mockReturnValue(of(dto({ count: 5 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBe('+7 vs today');
    expect(controller.deltaTone()).toBe('warn');
  });

  it('formats zero delta as "±0 vs today" with neutral tone', () => {
    getAt.mockReturnValue(of(dto({ count: 10 })));
    getCurrent.mockReturnValue(of(dto({ count: 10 })));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBe('±0 vs today');
    expect(controller.deltaTone()).toBe('neutral');
  });

  it('mirrors mode signal', () => {
    getCurrent.mockReturnValue(of(dto()));

    const controller = new GoalMetricsController(service);
    controller.setGoalId('g1');
    controller.load('live', null);
    expect(controller.mode()).toBe('live');

    getAt.mockReturnValue(of(dto()));
    controller.load('review', '2026-04-15T18:30:00Z');
    expect(controller.mode()).toBe('review');
  });

  it('declares _reviewDelta as a computed signal, not a method (bug-130)', async () => {
    const { readFileSync } = await import('fs');
    const { join } = await import('path');
    const ts = readFileSync(
      join(__dirname, 'goal-metrics.controller.ts'),
      'utf8'
    );
    expect(ts).toMatch(/_reviewDelta\s*=\s*computed</);
    expect(ts).not.toMatch(/^\s*private\s+_reviewDelta\(\)/m);
  });

  it('does not expose applyHubUpdate — no SignalR hub wired (bug-145)', async () => {
    const { readFileSync } = await import('fs');
    const { join } = await import('path');
    const ts = readFileSync(
      join(__dirname, 'goal-metrics.controller.ts'),
      'utf8'
    );
    expect(ts).not.toMatch(/\bapplyHubUpdate\s*\(/);
    expect(ts).not.toMatch(/\bGoalProgressUpdated\b/);
  });
});
