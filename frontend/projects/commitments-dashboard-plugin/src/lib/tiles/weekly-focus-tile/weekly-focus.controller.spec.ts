import { WeeklyFocusController } from './weekly-focus.controller';
import { WeeklyFocusDto, WeeklyFocusService } from '../../data/weekly-focus.service';

function dtoFixture(overrides: Partial<WeeklyFocusDto> = {}): WeeklyFocusDto {
  return {
    mode: 'live',
    asOf: '2026-04-27T18:00:00Z',
    weekStart: '2026-04-20',
    weekEnd: '2026-04-26',
    focusAreas: [
      { name: 'Move', supportingMetric: '5 sessions logged', rank: 1 },
      { name: 'Read', supportingMetric: '3 sessions logged', rank: 2 },
      { name: 'Reflect', supportingMetric: '2 notes logged', rank: 3 }
    ],
    ...overrides
  };
}

describe('WeeklyFocusController', () => {
  let get: jest.Mock;
  let service: WeeklyFocusService;

  beforeEach(() => {
    get = jest.fn();
    service = { get } as unknown as WeeklyFocusService;
  });

  it('starts with empty focus areas and an empty week label', () => {
    const controller = new WeeklyFocusController(service);

    expect(controller.focusAreas()).toEqual([]);
    expect(controller.weekLabel()).toBe('');
  });

  it('exposes the loaded focus areas', async () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new WeeklyFocusController(service);
    await controller.load('live', null);

    expect(controller.focusAreas()).toHaveLength(3);
    expect(controller.focusAreas()[0].name).toBe('Move');
  });

  it('formats the week label as "MMM d – MMM d"', async () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new WeeklyFocusController(service);
    await controller.load('live', null);

    expect(controller.weekLabel()).toBe('Apr 20 – Apr 26');
  });

  it('mirrors the mode passed to load', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review' }));

    const controller = new WeeklyFocusController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.mode()).toBe('review');
  });

  it('passes asOf through to the service', () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new WeeklyFocusController(service);
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(get).toHaveBeenCalledWith('2026-04-15T18:30:00Z');
  });
});
