import { GoalTrendDto } from '@commitments/dashboard-plugin';

function makePoint(date: string, percentage: number) {
  return { date, completed: percentage, target: 100, percentage };
}

export const goalTrendFixture: GoalTrendDto = {
  goalId: 'g-1',
  mode: 'live',
  asOf: '2026-04-29T00:00:00Z',
  windowDays: 14,
  points: [
    makePoint('2026-04-16', 55),
    makePoint('2026-04-17', 60),
    makePoint('2026-04-18', 58),
    makePoint('2026-04-19', 65),
    makePoint('2026-04-20', 70),
    makePoint('2026-04-21', 68),
    makePoint('2026-04-22', 72),
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
  lowPercentage: 55,
  deltaLabel: '+33% vs prior 14d'
};
