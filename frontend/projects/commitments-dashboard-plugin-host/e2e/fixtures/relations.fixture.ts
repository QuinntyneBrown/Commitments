import { RelationsSummaryDto } from '@commitments/dashboard-plugin';

export const relationsFixture: RelationsSummaryDto = {
  mode: 'live',
  asOf: '2026-04-29T00:00:00Z',
  totalCommitments: 20,
  relations: [
    { behaviourTypeId: 'health', name: 'Health', count: 8, percentage: 40 },
    { behaviourTypeId: 'learning', name: 'Learning', count: 6, percentage: 30 },
    { behaviourTypeId: 'social', name: 'Social', count: 6, percentage: 30 }
  ]
};
