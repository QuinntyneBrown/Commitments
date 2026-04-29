import { OutstandingTodosController } from './outstanding-todos.controller';
import { OutstandingTodosDto, OutstandingTodosService } from '../../data/outstanding-todos.service';

function dtoFixture(overrides: Partial<OutstandingTodosDto> = {}): OutstandingTodosDto {
  return {
    mode: 'live',
    asOf: '2026-04-27T17:00:00Z',
    count: 4,
    ...overrides
  };
}

describe('OutstandingTodosController', () => {
  let get: jest.Mock;
  let service: OutstandingTodosService;

  beforeEach(() => {
    get = jest.fn();
    service = { get } as unknown as OutstandingTodosService;
  });

  it('starts with count 0 and no delta', () => {
    const controller = new OutstandingTodosController(service);

    expect(controller.count()).toBe(0);
    expect(controller.deltaLabel()).toBeNull();
    expect(controller.deltaTone()).toBe('neutral');
  });

  it('exposes count from the loaded snapshot', async () => {
    get.mockResolvedValue(dtoFixture({ count: 7 }));

    const controller = new OutstandingTodosController(service);
    await controller.load('live', null);

    expect(controller.count()).toBe(7);
  });

  it('passes asOf and includeToday=true to service in review mode', () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new OutstandingTodosController(service);
    controller.load('review', '2026-04-15T18:30:00Z');

    expect(get).toHaveBeenCalledWith('review', '2026-04-15T18:30:00Z');
  });

  it('passes null asOf in live mode', () => {
    get.mockResolvedValue(dtoFixture());

    const controller = new OutstandingTodosController(service);
    controller.load('live', null);

    expect(get).toHaveBeenCalledWith('live', null);
  });

  it('returns null delta when not in review mode', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'live', count: 4, todayCount: 4 }));

    const controller = new OutstandingTodosController(service);
    await controller.load('live', null);

    expect(controller.deltaLabel()).toBeNull();
  });

  it('returns null delta when todayCount is missing', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review', count: 6 }));

    const controller = new OutstandingTodosController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBeNull();
  });

  it('formats positive delta as "+N vs today"', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review', count: 6, todayCount: 4 }));

    const controller = new OutstandingTodosController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBe('+2 vs today');
    expect(controller.deltaTone()).toBe('success');
  });

  it('formats negative delta as "-N vs today" with warn tone', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review', count: 2, todayCount: 5 }));

    const controller = new OutstandingTodosController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBe('-3 vs today');
    expect(controller.deltaTone()).toBe('warn');
  });

  it('formats zero delta as "±0 vs today" with neutral tone', async () => {
    get.mockResolvedValue(dtoFixture({ mode: 'review', count: 4, todayCount: 4 }));

    const controller = new OutstandingTodosController(service);
    await controller.load('review', '2026-04-15T18:30:00Z');

    expect(controller.deltaLabel()).toBe('±0 vs today');
    expect(controller.deltaTone()).toBe('neutral');
  });

  it('declares _reviewDelta as a computed signal, not a method (bug-131)', async () => {
    const { readFileSync } = await import('fs');
    const { join } = await import('path');
    const ts = readFileSync(
      join(__dirname, 'outstanding-todos.controller.ts'),
      'utf8'
    );
    expect(ts).toMatch(/_reviewDelta\s*=\s*computed</);
    expect(ts).not.toMatch(/^\s*private\s+_reviewDelta\(\)/m);
  });
});
