import { HubRetryPolicy } from './hub-retry-policy';

describe('HubRetryPolicy', () => {
  function context(previousRetryCount: number, elapsedMilliseconds = 0) {
    return { previousRetryCount, elapsedMilliseconds, retryReason: new Error('drop') };
  }

  it('returns the documented backoff sequence: 0ms, 2s, 5s, 10s, 30s', () => {
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(0))).toBe(0);
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(1))).toBe(2_000);
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(2))).toBe(5_000);
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(3))).toBe(10_000);
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(4))).toBe(30_000);
  });

  it('caps at 60s once the backoff sequence is exhausted', () => {
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(5, 60_000))).toBe(60_000);
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(20, 200_000))).toBe(60_000);
  });

  it('gives up after 5 minutes of total wall time', () => {
    expect(HubRetryPolicy.nextRetryDelayInMilliseconds(context(10, 5 * 60_000 + 1))).toBeNull();
  });
});
