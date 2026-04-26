import { DashboardModeService, MODE_STORAGE_KEY, REVIEW_DATE_STORAGE_KEY } from './dashboard-mode.service';

describe('DashboardModeService', () => {
  beforeEach(() => {
    localStorage.clear();
  });

  it('defaults to live mode when storage is empty', () => {
    const service = new DashboardModeService();
    expect(service.mode()).toBe('live');
    expect(service.selectedReviewDate()).toBeNull();
  });

  it('persists the selected mode to localStorage', () => {
    const service = new DashboardModeService();
    service.setMode('review');
    expect(localStorage.getItem(MODE_STORAGE_KEY)).toBe('review');
    expect(service.mode()).toBe('review');
  });

  it('restores the persisted mode on construction', () => {
    localStorage.setItem(MODE_STORAGE_KEY, 'review');
    const service = new DashboardModeService();
    expect(service.mode()).toBe('review');
  });

  it('falls back to live when the stored value is invalid', () => {
    localStorage.setItem(MODE_STORAGE_KEY, 'garbage-value');
    const service = new DashboardModeService();
    expect(service.mode()).toBe('live');
  });

  it('defaults selectedReviewDate to today on first switch to review', () => {
    const service = new DashboardModeService();
    service.setMode('review');
    expect(service.selectedReviewDate()).toMatch(/^\d{4}-\d{2}-\d{2}$/);
  });

  it('persists and restores selectedReviewDate', () => {
    const service = new DashboardModeService();
    service.setSelectedReviewDate('2026-04-10');
    expect(localStorage.getItem(REVIEW_DATE_STORAGE_KEY)).toBe('2026-04-10');

    const service2 = new DashboardModeService();
    expect(service2.selectedReviewDate()).toBe('2026-04-10');
  });
});
