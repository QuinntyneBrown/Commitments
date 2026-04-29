import { WeeklyFocusDto } from '@commitments/dashboard-plugin';

export const weeklyFocusReviewFixture: WeeklyFocusDto = {
  mode: 'review',
  asOf: '2026-04-01T00:00:00Z',
  weekStart: '2026-03-30',
  weekEnd: '2026-04-05',
  focusAreas: [
    { name: 'Fitness', supportingMetric: '4 sessions planned', rank: 1 },
    { name: 'Study', supportingMetric: '45 min daily', rank: 2 }
  ]
};
