import {
  ACCENT_CHART,
  ACCENT_LIVE,
  ACCENT_REVIEW,
  ACCENT_SUCCESS,
  BG_APP,
  BG_SIDEBAR,
  BG_TOOLBAR,
  DIVIDER,
  SURFACE_RAISED,
  SURFACE_TILE,
  TEXT_MUTED,
  TEXT_PRIMARY,
  TEXT_SECONDARY
} from './tokens';

describe('tokens', () => {
  it('exposes the dark theme palette from ui-design.pen', () => {
    expect(BG_APP).toBe('#121212');
    expect(BG_TOOLBAR).toBe('#1F2233');
    expect(BG_SIDEBAR).toBe('#181A24');
    expect(SURFACE_TILE).toBe('#242424');
    expect(SURFACE_RAISED).toBe('#1E1E1E');
    expect(DIVIDER).toBe('#3A3A3A');
  });

  it('exposes the accent palette', () => {
    expect(ACCENT_LIVE).toBe('#FF4081');
    expect(ACCENT_REVIEW).toBe('#9FA8DA');
    expect(ACCENT_CHART).toBe('#42A5F5');
    expect(ACCENT_SUCCESS).toBe('#66BB6A');
  });

  it('exposes the text palette', () => {
    expect(TEXT_PRIMARY).toBe('#FFFFFF');
    expect(TEXT_SECONDARY).toBe('#B0B0B0');
    expect(TEXT_MUTED).toBe('#666666');
  });
});
