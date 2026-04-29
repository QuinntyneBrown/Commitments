import { RelationsController } from './relations.controller';
import { RelationsSummaryDto, RelationsSummaryService } from '../../data/relations-summary.service';

function dtoFixture(overrides: Partial<RelationsSummaryDto> = {}): RelationsSummaryDto {
  return {
    mode: 'live',
    asOf: '2026-04-27T17:00:00Z',
    totalCommitments: 12,
    relations: [
      { behaviourTypeId: 'h', name: 'Health', count: 5, percentage: 42 },
      { behaviourTypeId: 'w', name: 'Work', count: 4, percentage: 33 },
      { behaviourTypeId: 'p', name: 'Personal', count: 3, percentage: 25 }
    ],
    ...overrides
  };
}

describe('RelationsController', () => {
  let get: jest.Mock;
  let service: RelationsSummaryService;

  beforeEach(() => {
    get = jest.fn();
    service = { get } as unknown as RelationsSummaryService;
  });

  it('starts empty and isEmpty=true', () => {
    const controller = new RelationsController(service);

    expect(controller.relations()).toEqual([]);
    expect(controller.isEmpty()).toBe(true);
  });

  it('exposes the loaded relations', async () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new RelationsController(service);
    await controller.load('live', null);

    expect(controller.relations()).toHaveLength(3);
    expect(controller.relations()[0].name).toBe('Health');
    expect(controller.isEmpty()).toBe(false);
  });

  it('mirrors the mode passed to load', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review' }));

    const controller = new RelationsController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.mode()).toBe('review');
  });

  it('passes asOf through to the service', () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new RelationsController(service);
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(get).toHaveBeenCalledWith('2026-04-15T18:30:00Z');
  });

  it('reports isEmpty=true when relations array is empty', async () => {
    get.mockResolvedValue(dtoFixture({ relations: [], totalCommitments: 0 }));

    const controller = new RelationsController(service);
    await controller.load('live', null);

    expect(controller.isEmpty()).toBe(true);
  });
});
