import {
  clampIndex,
  indexOfDate,
  isoDateAtIndex,
  tickLabels
} from './review-scrubber.helpers';

describe('review-scrubber.helpers', () => {
  describe('clampIndex', () => {
    it('returns the value when within range', () => {
      expect(clampIndex(5, 10)).toBe(5);
    });

    it('clamps below to zero', () => {
      expect(clampIndex(-3, 10)).toBe(0);
    });

    it('clamps above to max', () => {
      expect(clampIndex(99, 10)).toBe(10);
    });
  });

  describe('isoDateAtIndex', () => {
    it('returns the start date at index 0', () => {
      expect(isoDateAtIndex('2026-04-01', 0)).toBe('2026-04-01');
    });

    it('returns the start + N days at index N', () => {
      expect(isoDateAtIndex('2026-04-01', 9)).toBe('2026-04-10');
    });

    it('crosses month boundaries correctly', () => {
      expect(isoDateAtIndex('2026-03-30', 5)).toBe('2026-04-04');
    });
  });

  describe('indexOfDate', () => {
    it('returns 0 for the start date', () => {
      expect(indexOfDate('2026-04-01', '2026-04-01')).toBe(0);
    });

    it('returns the day offset', () => {
      expect(indexOfDate('2026-04-01', '2026-04-10')).toBe(9);
    });
  });

  describe('tickLabels', () => {
    it('returns no more labels than the count', () => {
      const labels = tickLabels('2026-04-01', 30, 600);
      expect(labels.length).toBeLessThanOrEqual(30);
      expect(labels.length).toBeGreaterThan(0);
    });

    it('always includes the first day', () => {
      const labels = tickLabels('2026-04-01', 30, 600);
      expect(labels[0]).toContain('Apr');
    });
  });
});
