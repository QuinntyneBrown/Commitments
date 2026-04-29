import { WeeklyFocusDto } from '@commitments/dashboard-plugin';

export const weeklyFocusFixture: WeeklyFocusDto = {
  mode: 'live',
  asOf: '2026-04-29T00:00:00Z',
  weekStart: '2026-04-28',
  weekEnd: '2026-05-04',
  focusAreas: [
    { name: 'Exercise', supportingMetric: '5 sessions planned', rank: 1 },
    { name: 'Reading', supportingMetric: '30 min daily', rank: 2 },
    { name: 'Deep work', supportingMetric: '2 h blocks', rank: 3 }
  ]
};
