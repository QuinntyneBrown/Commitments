import { MonthlyProgressController } from './monthly-progress.controller';
import { MonthlyProgressDto, MonthlyProgressService } from '../../data/monthly-progress.service';

function dtoFixture(overrides: Partial<MonthlyProgressDto> = {}): MonthlyProgressDto {
  return {
    mode: 'live',
    asOf: '2026-04-27T17:00:00Z',
    windowDays: 30,
    buckets: [
      { weekStart: '2026-03-30', weekEnd: '2026-04-06', completed: 21, target: 60, percentage: 35 },
      { weekStart: '2026-04-06', weekEnd: '2026-04-13', completed: 43, target: 60, percentage: 72 },
      { weekStart: '2026-04-13', weekEnd: '2026-04-20', completed: 35, target: 60, percentage: 58 },
      { weekStart: '2026-04-20', weekEnd: '2026-04-27', completed: 52, target: 60, percentage: 86 }
    ],
    isEmpty: false,
    ...overrides
  };
}

describe('MonthlyProgressController', () => {
  let get: jest.Mock;
  let service: MonthlyProgressService;

  beforeEach(() => {
    get = jest.fn();
    service = { get } as unknown as MonthlyProgressService;
  });

  it('starts with empty buckets and isEmpty=true', () => {
    const controller = new MonthlyProgressController(service);

    expect(controller.buckets()).toEqual([]);
    expect(controller.isEmpty()).toBe(true);
  });

  it('exposes the four loaded buckets', async () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new MonthlyProgressController(service);
    await controller.load('live', null);

    expect(controller.buckets()).toHaveLength(4);
    expect(controller.buckets()[3].percentage).toBe(86);
  });

  it('mirrors the snapshot isEmpty flag', async () => {
    get.mockResolvedValue(dtoFixture({
      buckets: dtoFixture().buckets.map(b => ({ ...b, completed: 0, percentage: 0 })),
      isEmpty: true
    }));

    const controller = new MonthlyProgressController(service);
    await controller.load('live', null);

    expect(controller.isEmpty()).toBe(true);
  });

  it('mirrors the mode passed to load', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review' }));

    const controller = new MonthlyProgressController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.mode()).toBe('review');
  });

  it('passes asOf through to the service', () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new MonthlyProgressController(service);
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(get).toHaveBeenCalledWith('2026-04-15T18:30:00Z');
  });
});
