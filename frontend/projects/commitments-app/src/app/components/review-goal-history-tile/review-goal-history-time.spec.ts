import { localInputToIso, scrubInstantFromPct } from './review-goal-history-time';

describe('review-goal-history-time helpers', () => {
  describe('scrubInstantFromPct', () => {
    it('returns the start when pct is 0', () => {
      expect(scrubInstantFromPct(1000, 5000, 0)).toBe(1000);
    });

    it('returns the end when pct is 100', () => {
      expect(scrubInstantFromPct(1000, 5000, 100)).toBe(5000);
    });

    it('interpolates linearly at 50%', () => {
      expect(scrubInstantFromPct(0, 1000, 50)).toBe(500);
    });

    it('clamps pct below 0 to 0', () => {
      expect(scrubInstantFromPct(0, 1000, -25)).toBe(0);
    });

    it('clamps pct above 100 to 100', () => {
      expect(scrubInstantFromPct(0, 1000, 250)).toBe(1000);
    });
  });

  describe('localInputToIso', () => {
    it('converts a datetime-local value to an ISO string', () => {
      const result = localInputToIso('2026-04-10T15:30');
      expect(result).not.toBeNull();
      expect(new Date(result as string).toISOString()).toMatch(/^2026-04-10T/);
    });

    it('returns null for an empty input', () => {
      expect(localInputToIso('')).toBeNull();
    });

    it('returns null for an invalid input', () => {
      expect(localInputToIso('not-a-date')).toBeNull();
    });
  });
});
