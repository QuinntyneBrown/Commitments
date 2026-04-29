import { GoalTrendDto } from '@commitments/dashboard-plugin';

function makePoint(date: string, percentage: number) {
  return { date, completed: percentage, target: 100, percentage };
}

export const goalTrendFixture: GoalTrendDto = {
  goalId: 'demo-goal',
  mode: 'live',
  asOf: '2026-04-29T00:00:00Z',
  windowDays: 7,
  points: [
    makePoint('2026-04-23', 70),
    makePoint('2026-04-24', 75),
    makePoint('2026-04-25', 80),
    makePoint('2026-04-26', 72),
    makePoint('2026-04-27', 85),
    makePoint('2026-04-28', 90),
    makePoint('2026-04-29', 88)
  ],
  currentPercentage: 88,
  peakPercentage: 90,
  lowPercentage: 70,
  deltaLabel: '+18% this week'
};
