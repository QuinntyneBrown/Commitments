import { DashboardModeService } from '../dashboard-mode.service';

import { ReviewScrubberController } from './review-scrubber.controller';

describe('ReviewScrubberController', () => {
  let modeService: DashboardModeService;

  beforeEach(() => {
    localStorage.clear();
    modeService = new DashboardModeService();
    modeService.setMode('review');
  });

  it('exposes a 90-day window by default', () => {
    const controller = new ReviewScrubberController(modeService);
    expect(controller.windowDays()).toBe(90);
  });

  it('next advances the selected date by one day, clamped to windowEnd', () => {
    const controller = new ReviewScrubberController(modeService, 5);
    controller.jumpToStart();
    const startIndex = controller.selectedIndex();

    controller.next();

    expect(controller.selectedIndex()).toBe(startIndex + 1);
  });

  it('next stops at the last index', () => {
    const controller = new ReviewScrubberController(modeService, 5);
    controller.scrubTo(4, 0);
    controller.next();
    expect(controller.selectedIndex()).toBe(4);
  });

  it('prev steps back one day, clamped to 0', () => {
    const controller = new ReviewScrubberController(modeService, 5);
    controller.scrubTo(2, 0);
    controller.prev();
    expect(controller.selectedIndex()).toBe(1);

    controller.prev();
    controller.prev();
    expect(controller.selectedIndex()).toBe(0);

    controller.prev();
    expect(controller.selectedIndex()).toBe(0);
  });

  it('jumpToToday sets selectedDate to windowEnd (today)', () => {
    const controller = new ReviewScrubberController(modeService, 14);
    controller.scrubTo(2, 0);
    controller.jumpToToday();
    expect(controller.selectedIndex()).toBe(13);
    expect(controller.selectedDate()).toBe(controller.windowEnd());
  });

  it('scrubTo writes through to DashboardModeService when debounce is 0', () => {
    const controller = new ReviewScrubberController(modeService, 5);
    controller.scrubTo(2, 0);
    expect(modeService.selectedReviewDate()).toBe(controller.selectedDate());
  });

  it('scrubTo debounces the write to DashboardModeService', () => {
    jest.useFakeTimers();
    try {
      const controller = new ReviewScrubberController(modeService, 5);
      const before = modeService.selectedReviewDate();
      controller.scrubTo(1);
      controller.scrubTo(2);
      controller.scrubTo(3);

      expect(modeService.selectedReviewDate()).toBe(before);

      jest.advanceTimersByTime(150);

      expect(modeService.selectedReviewDate()).not.toBe(before);
      expect(controller.selectedIndex()).toBe(3);
    } finally {
      jest.useRealTimers();
    }
  });
});
