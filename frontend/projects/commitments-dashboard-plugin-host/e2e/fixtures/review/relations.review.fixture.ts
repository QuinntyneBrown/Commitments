import { RelationsSummaryDto } from '@commitments/dashboard-plugin';

export const relationsReviewFixture: RelationsSummaryDto = {
  mode: 'review',
  asOf: '2026-04-01T00:00:00Z',
  totalCommitments: 18,
  relations: [
    { behaviourTypeId: 'health', name: 'Health', count: 7, percentage: 39 },
    { behaviourTypeId: 'learning', name: 'Learning', count: 6, percentage: 33 },
    { behaviourTypeId: 'social', name: 'Social', count: 5, percentage: 28 }
  ]
};
