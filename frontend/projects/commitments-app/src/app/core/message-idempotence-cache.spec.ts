import { MessageIdempotenceCache } from './message-idempotence-cache';

describe('MessageIdempotenceCache', () => {
  it('returns false on first sighting of an id', () => {
    const cache = new MessageIdempotenceCache();
    expect(cache.seen('abc')).toBe(false);
  });

  it('returns true on second sighting of the same id', () => {
    const cache = new MessageIdempotenceCache();
    cache.seen('abc');
    expect(cache.seen('abc')).toBe(true);
  });

  it('returns false for null or undefined ids (legacy messages without messageId)', () => {
    const cache = new MessageIdempotenceCache();
    expect(cache.seen(null)).toBe(false);
    expect(cache.seen(undefined)).toBe(false);
    expect(cache.seen('')).toBe(false);
  });

  it('evicts oldest id once capacity is exceeded (FIFO)', () => {
    const cache = new MessageIdempotenceCache(2);
    cache.seen('a');
    cache.seen('b');
    cache.seen('c');

    expect(cache.seen('b')).toBe(true);
    expect(cache.seen('c')).toBe(true);
    expect(cache.seen('a')).toBe(false);
  });
});
