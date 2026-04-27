import { DailyResultsController } from './daily-results.controller';
import { DailyResultsDto, DailyResultsService } from '../../data/daily-results.service';

function dtoFixture(overrides: Partial<DailyResultsDto> = {}): DailyResultsDto {
  return {
    mode: 'live',
    asOf: '2026-04-27T18:00:00Z',
    date: '2026-04-27',
    completed: 7,
    total: 9,
    ...overrides
  };
}

describe('DailyResultsController', () => {
  let get: jest.Mock;
  let service: DailyResultsService;

  beforeEach(() => {
    get = jest.fn();
    service = { get } as unknown as DailyResultsService;
  });

  it('starts with zero completed/total/percentage', () => {
    const controller = new DailyResultsController(service);

    expect(controller.completed()).toBe(0);
    expect(controller.total()).toBe(0);
    expect(controller.percentage()).toBe(0);
  });

  it('exposes completed/total/percentage from the loaded snapshot', () => {
    const dto = dtoFixture({ completed: 7, total: 9 });
    get.mockReturnValue({ subscribe: (fn: (v: DailyResultsDto) => void) => fn(dto) });

    const controller = new DailyResultsController(service);
    controller.load('live', null);

    expect(controller.completed()).toBe(7);
    expect(controller.total()).toBe(9);
    expect(controller.percentage()).toBe(78);
  });

  it('mirrors the mode passed to load', () => {
    get.mockReturnValue({
      subscribe: (fn: (v: DailyResultsDto) => void) => fn(dtoFixture({ mode: 'review' }))
    });

    const controller = new DailyResultsController(service);
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.mode()).toBe('review');
  });

  it('passes asOf through to the service', () => {
    get.mockReturnValue({ subscribe: () => {} });

    const controller = new DailyResultsController(service);
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(get).toHaveBeenCalledWith('2026-04-15T18:30:00Z');
  });

  it('percentage is zero when total is zero', () => {
    get.mockReturnValue({
      subscribe: (fn: (v: DailyResultsDto) => void) => fn(dtoFixture({ completed: 0, total: 0 }))
    });

    const controller = new DailyResultsController(service);
    controller.load('live', null);

    expect(controller.percentage()).toBe(0);
  });

  it('percentage rounds to nearest integer', () => {
    get.mockReturnValue({
      subscribe: (fn: (v: DailyResultsDto) => void) => fn(dtoFixture({ completed: 1, total: 3 }))
    });

    const controller = new DailyResultsController(service);
    controller.load('live', null);

    expect(controller.percentage()).toBe(33);
  });
});
