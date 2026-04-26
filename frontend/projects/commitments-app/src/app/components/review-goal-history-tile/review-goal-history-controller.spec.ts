import { signal } from '@angular/core';

import { GoalProgress, GoalProgressService } from '../../services/goal-progress.service';

import { ReviewGoalHistoryController } from './review-goal-history-controller';

function progress(count: number, target: number = 30, asOf: string = '2026-04-26'): GoalProgress {
  return { goalId: 'goal-1', target, count, asOf };
}

describe('ReviewGoalHistoryController', () => {
  let getAt: jest.Mock;
  let getCurrent: jest.Mock;
  let goalProgressService: GoalProgressService;
  let selectedReviewDate: ReturnType<typeof signal<string | null>>;

  beforeEach(() => {
    getAt = jest.fn();
    getCurrent = jest.fn();
    goalProgressService = { getAt, getCurrent } as unknown as GoalProgressService;
    selectedReviewDate = signal<string | null>('2026-04-12');
  });

  it('loads historical and today snapshots on load', () => {
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(7)) });
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(15)) });

    const controller = new ReviewGoalHistoryController(goalProgressService, selectedReviewDate);
    controller.load('goal-1');

    expect(getAt).toHaveBeenCalledTimes(1);
    expect(getCurrent).toHaveBeenCalledTimes(1);
    expect(controller.count()).toBe(7);
    expect(controller.target()).toBe(30);
  });

  it('computes delta as historical.count - today.count', () => {
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(8)) });
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(20)) });

    const controller = new ReviewGoalHistoryController(goalProgressService, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.delta()).toBe(-12);
  });

  it('falls back to today when selectedReviewDate is null', () => {
    selectedReviewDate.set(null);
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(0)) });
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(0)) });

    const controller = new ReviewGoalHistoryController(goalProgressService, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.selectedDate()).toMatch(/^\d{4}-\d{2}-\d{2}$/);
  });

  it('renders empty day with count 0 and target intact', () => {
    getAt.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(0, 30)) });
    getCurrent.mockReturnValue({ subscribe: (fn: (v: GoalProgress) => void) => fn(progress(5, 30)) });

    const controller = new ReviewGoalHistoryController(goalProgressService, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.count()).toBe(0);
    expect(controller.target()).toBe(30);
    expect(controller.isEmpty()).toBe(true);
  });
});
