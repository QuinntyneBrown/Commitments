import { LayoutPersistenceService, LIVE_LAYOUT_STORAGE_KEY, REVIEW_LAYOUT_STORAGE_KEY } from './layout-persistence.service';
import { PersistedLayout } from './dashboard.model';

describe('LayoutPersistenceService', () => {
  let service: LayoutPersistenceService;

  beforeEach(() => {
    localStorage.clear();
    service = new LayoutPersistenceService();
  });

  function fixture(items: PersistedLayout['items']): PersistedLayout {
    return { schemaVersion: 1, savedAt: 0, items };
  }

  it('saves and loads the live layout under the live storage key', () => {
    const layout = fixture([
      { instanceId: 'a', tileId: 't1', cols: 3, rows: 2, x: 0, y: 0 }
    ]);

    service.save('live', layout);

    expect(localStorage.getItem(LIVE_LAYOUT_STORAGE_KEY)).not.toBeNull();
    expect(localStorage.getItem(REVIEW_LAYOUT_STORAGE_KEY)).toBeNull();
    expect(service.load('live')?.items).toEqual(layout.items);
  });

  it('saves and loads the review layout under the review storage key', () => {
    const layout = fixture([
      { instanceId: 'b', tileId: 't2', cols: 3, rows: 2, x: 0, y: 0 }
    ]);

    service.save('review', layout);

    expect(localStorage.getItem(REVIEW_LAYOUT_STORAGE_KEY)).not.toBeNull();
    expect(localStorage.getItem(LIVE_LAYOUT_STORAGE_KEY)).toBeNull();
    expect(service.load('review')?.items).toEqual(layout.items);
  });

  it('returns null when the requested mode has nothing stored', () => {
    expect(service.load('live')).toBeNull();
    expect(service.load('review')).toBeNull();
  });

  it('returns null when the stored value is invalid JSON', () => {
    localStorage.setItem(LIVE_LAYOUT_STORAGE_KEY, 'not-json');
    expect(service.load('live')).toBeNull();
  });

  it('clear only removes the requested mode key', () => {
    service.save('live', fixture([]));
    service.save('review', fixture([]));

    service.clear('live');

    expect(localStorage.getItem(LIVE_LAYOUT_STORAGE_KEY)).toBeNull();
    expect(localStorage.getItem(REVIEW_LAYOUT_STORAGE_KEY)).not.toBeNull();
  });
});
