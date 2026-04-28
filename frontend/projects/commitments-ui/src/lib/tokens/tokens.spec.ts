import { ACCENT_CHART, BG_APP, TEXT_MUTED } from './tokens';

describe('tokens', () => {
  it('exposes the canvas palette mirroring _tokens.scss', () => {
    expect(BG_APP).toBe('#121212');
    expect(ACCENT_CHART).toBe('#42A5F5');
    expect(TEXT_MUTED).toBe('#666666');
  });
});
