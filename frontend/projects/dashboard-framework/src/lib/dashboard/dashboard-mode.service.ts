import { Injectable, Signal, signal } from '@angular/core';

import { DashboardMode } from '../tile-registration/tile.model';

export const MODE_STORAGE_KEY = 'commitments.dashboardMode';
export const REVIEW_DATE_STORAGE_KEY = 'commitments.selectedReviewDate';

const VALID_MODES: ReadonlyArray<DashboardMode> = ['live', 'review'];

function todayIsoDate(): string {
  return new Date().toISOString().slice(0, 10);
}

function readStoredMode(): DashboardMode {
  const stored = localStorage.getItem(MODE_STORAGE_KEY) as DashboardMode | null;
  return stored && VALID_MODES.includes(stored) ? stored : 'live';
}

@Injectable({ providedIn: 'root' })
export class DashboardModeService {
  private readonly _mode = signal<DashboardMode>(readStoredMode());
  private readonly _selectedReviewDate = signal<string | null>(localStorage.getItem(REVIEW_DATE_STORAGE_KEY));

  readonly mode: Signal<DashboardMode> = this._mode.asReadonly();
  readonly selectedReviewDate: Signal<string | null> = this._selectedReviewDate.asReadonly();

  setMode(mode: DashboardMode): void {
    this._mode.set(mode);
    localStorage.setItem(MODE_STORAGE_KEY, mode);
    if (mode === 'review' && this._selectedReviewDate() === null) {
      this.setSelectedReviewDate(todayIsoDate());
    }
  }

  setSelectedReviewDate(date: string): void {
    this._selectedReviewDate.set(date);
    localStorage.setItem(REVIEW_DATE_STORAGE_KEY, date);
  }
}
