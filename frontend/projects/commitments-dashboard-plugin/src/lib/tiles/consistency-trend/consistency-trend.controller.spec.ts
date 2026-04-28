import { signal } from '@angular/core';
import { readFileSync } from 'fs';
import { join } from 'path';

import { ConsistencyTrendController } from './consistency-trend.controller';
import { GoalTrendDto, GoalTrendService } from '../../data/goal-trend.service';

function trendFixture(overrides: Partial<GoalTrendDto> = {}): GoalTrendDto {
  return {
    goalId: 'goal-1',
    mode: 'live',
    asOf: '2026-04-26',
    windowDays: 5,
    points: [
      { date: '2026-04-22', completed: 1, target: 30, percentage: 3 },
      { date: '2026-04-23', completed: 2, target: 30, percentage: 7 },
      { date: '2026-04-24', completed: 4, target: 30, percentage: 13 },
      { date: '2026-04-25', completed: 5, target: 30, percentage: 17 },
      { date: '2026-04-26', completed: 6, target: 30, percentage: 20 }
    ],
    currentPercentage: 20,
    peakPercentage: 20,
    lowPercentage: 3,
    deltaLabel: '+3% vs prior 14d',
    ...overrides
  };
}

describe('ConsistencyTrendController', () => {
  let getTrend: jest.Mock;
  let trendService: GoalTrendService;
  let mode: ReturnType<typeof signal<'live' | 'review'>>;
  let selectedReviewDate: ReturnType<typeof signal<string | null>>;

  beforeEach(() => {
    mode = signal<'live' | 'review'>('live');
    selectedReviewDate = signal<string | null>(null);
    getTrend = jest.fn();
    trendService = { getTrend } as unknown as GoalTrendService;
  });

  it('exposes summary signals from the loaded trend', () => {
    const trend = trendFixture();
    getTrend.mockReturnValue({ subscribe: (fn: (v: GoalTrendDto) => void) => fn(trend) });

    const controller = new ConsistencyTrendController(trendService, mode, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.currentPercentage()).toBe(20);
    expect(controller.peakPercentage()).toBe(20);
    expect(controller.lowPercentage()).toBe(3);
    expect(controller.deltaLabel()).toBe('+3% vs prior 14d');
  });

  it('highlights the last point in live mode', () => {
    const trend = trendFixture();
    getTrend.mockReturnValue({ subscribe: (fn: (v: GoalTrendDto) => void) => fn(trend) });

    const controller = new ConsistencyTrendController(trendService, mode, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.highlightedIndex()).toBe(4);
  });

  it('highlights the matching date in review mode', () => {
    mode.set('review');
    selectedReviewDate.set('2026-04-24');
    const trend = trendFixture({ mode: 'review' });
    getTrend.mockReturnValue({ subscribe: (fn: (v: GoalTrendDto) => void) => fn(trend) });

    const controller = new ConsistencyTrendController(trendService, mode, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.highlightedIndex()).toBe(2);
  });

  it('returns -1 highlightedIndex when the selected date is not in the window', () => {
    mode.set('review');
    selectedReviewDate.set('2025-01-01');
    const trend = trendFixture({ mode: 'review' });
    getTrend.mockReturnValue({ subscribe: (fn: (v: GoalTrendDto) => void) => fn(trend) });

    const controller = new ConsistencyTrendController(trendService, mode, selectedReviewDate);
    controller.load('goal-1');

    expect(controller.highlightedIndex()).toBe(-1);
  });

  it('toChartDataset returns a non-empty data array', () => {
    const trend = trendFixture();
    getTrend.mockReturnValue({ subscribe: (fn: (v: GoalTrendDto) => void) => fn(trend) });

    const controller = new ConsistencyTrendController(trendService, mode, selectedReviewDate);
    controller.load('goal-1');

    const dataset = controller.chartDataset();
    expect(dataset.data.length).toBe(5);
    expect(dataset.data[4]).toBe(20);
  });

  it('uses a vertical gradient for the area fill (bug-044)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend.controller.ts'),
      'utf8'
    );
    // backgroundColor must be a function that builds a gradient at draw
    // time, not a flat colour string.
    expect(ts).toMatch(/backgroundColor\s*:\s*\([^)]*\)\s*=>/);
    expect(ts).toMatch(/createLinearGradient\b/);
  });

  it('strokes only the highlighted point with BG_APP (bug-045)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend.controller.ts'),
      'utf8'
    );
    // pointBorderColor and pointBorderWidth should now be arrays/functions
    // referencing BG_APP and a 3-px highlight width.
    expect(ts).toMatch(/pointBorderColor\s*:[\s\S]*?BG_APP/);
    expect(ts).toMatch(/pointBorderWidth\s*:[\s\S]*?\b3\b/);
  });

  describe('deltaPercentage / deltaCaption (bug-055)', () => {
    function loadWithLabel(label: string) {
      const trend = trendFixture({ deltaLabel: label });
      getTrend.mockReturnValue({ subscribe: (fn: (v: GoalTrendDto) => void) => fn(trend) });
      const controller = new ConsistencyTrendController(trendService, mode, selectedReviewDate);
      controller.load('goal-1');
      return controller;
    }

    it('parses a positive deltaPercentage', () => {
      expect(loadWithLabel('+12% vs prior 14d').deltaPercentage()).toBe(12);
    });

    it('parses a negative deltaPercentage', () => {
      expect(loadWithLabel('-3% vs prior 14d').deltaPercentage()).toBe(-3);
    });

    it('parses zero deltaPercentage', () => {
      expect(loadWithLabel('±0% vs prior 14d').deltaPercentage()).toBe(0);
    });

    it('extracts the caption suffix after the percent sign', () => {
      expect(loadWithLabel('+12% vs prior 14d').deltaCaption()).toBe('vs prior 14d');
    });
  });
});
