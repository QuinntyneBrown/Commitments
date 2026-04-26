import { GoalProgress, GoalProgressService } from '../../services/goal-progress.service';

import { ReviewGoalHistoryController } from './review-goal-history-controller';

describe('ReviewGoalHistoryController', () => {
  let getAt: jest.Mock;
  let goalProgressService: GoalProgressService;

  beforeEach(() => {
    getAt = jest.fn();
    goalProgressService = { getAt } as unknown as GoalProgressService;
  });

  it('canApply is false until both draft start and end are valid and ordered', () => {
    const controller = new ReviewGoalHistoryController(goalProgressService);

    expect(controller.canApply()).toBe(false);

    controller.setDraftStart('2026-04-01T00:00');
    controller.setDraftEnd('2026-04-10T00:00');

    expect(controller.canApply()).toBe(true);
  });

  it('canApply is false when draft end is before draft start', () => {
    const controller = new ReviewGoalHistoryController(goalProgressService);

    controller.setDraftStart('2026-04-10T00:00');
    controller.setDraftEnd('2026-04-01T00:00');

    expect(controller.canApply()).toBe(false);
  });

  it('apply pulls progress at the midpoint of the window', () => {
    const progress: GoalProgress = {
      goalId: 'goal-1',
      target: 30,
      count: 7,
      asOf: new Date('2026-04-05T12:00:00Z')
    };
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress) });

    const controller = new ReviewGoalHistoryController(goalProgressService);
    controller.load('goal-1');
    controller.setDraftStart('2026-04-01T00:00');
    controller.setDraftEnd('2026-04-10T00:00');
    controller.apply();

    expect(getAt).toHaveBeenCalledTimes(1);
    expect(controller.count()).toBe(7);
    expect(controller.target()).toBe(30);
  });

  it('scrub re-fetches progress at the new instant (without debounce)', () => {
    const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 3, asOf: new Date() };
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

    const controller = new ReviewGoalHistoryController(goalProgressService);
    controller.load('goal-1');
    controller.setDraftStart('2026-04-01T00:00');
    controller.setDraftEnd('2026-04-10T00:00');
    controller.apply();

    getAt.mockClear();
    const next: GoalProgress = { goalId: 'goal-1', target: 30, count: 9, asOf: new Date() };
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(next) });

    controller.scrub(75, 0);

    expect(getAt).toHaveBeenCalledTimes(1);
    expect(controller.count()).toBe(9);
    expect(controller.scrubPct()).toBe(75);
  });

  it('scrub debounces fetch calls when called rapidly', () => {
    jest.useFakeTimers();
    try {
      const initial: GoalProgress = { goalId: 'goal-1', target: 30, count: 3, asOf: new Date() };
      getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(initial) });

      const controller = new ReviewGoalHistoryController(goalProgressService);
      controller.load('goal-1');
      controller.setDraftStart('2026-04-01T00:00');
      controller.setDraftEnd('2026-04-10T00:00');
      controller.apply();

      getAt.mockClear();
      controller.scrub(25);
      controller.scrub(50);
      controller.scrub(75);

      expect(getAt).not.toHaveBeenCalled();

      jest.advanceTimersByTime(150);

      expect(getAt).toHaveBeenCalledTimes(1);
      expect(controller.scrubPct()).toBe(75);
    } finally {
      jest.useRealTimers();
    }
  });
});
