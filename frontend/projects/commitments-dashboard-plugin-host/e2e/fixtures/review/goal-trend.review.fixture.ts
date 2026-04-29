import { GoalTrendDto } from '@commitments/dashboard-plugin';

function makePoint(date: string, percentage: number) {
  return { date, completed: percentage, target: 100, percentage };
}

export const goalTrendReviewFixture: GoalTrendDto = {
  goalId: 'g-1',
  mode: 'review',
  asOf: '2026-03-28T00:00:00Z',
  windowDays: 14,
  points: [
    makePoint('2026-03-19', 50),
    makePoint('2026-03-20', 55),
    makePoint('2026-03-21', 48),
    makePoint('2026-03-22', 60),
    makePoint('2026-03-23', 62),
    makePoint('2026-03-24', 58),
    makePoint('2026-03-25', 65),
    makePoint('2026-03-26', 68),
    makePoint('2026-03-27', 70),
    makePoint('2026-03-28', 72),  // index 9 — matches asOf
    makePoint('2026-03-29', 74),
    makePoint('2026-03-30', 76),
    makePoint('2026-03-31', 78),
    makePoint('2026-04-01', 80)
  ],
  currentPercentage: 80,
  peakPercentage: 80,
  lowPercentage: 48,
  deltaLabel: '+22% vs prior 14d'
};
