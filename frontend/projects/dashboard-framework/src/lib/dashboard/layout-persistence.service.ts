import { Injectable } from '@angular/core';

import { DashboardMode } from '../tile-registration';

import {
  LIVE_LAYOUT_STORAGE_KEY,
  PersistedLayout,
  REVIEW_LAYOUT_STORAGE_KEY
} from './dashboard.model';

export { LIVE_LAYOUT_STORAGE_KEY, REVIEW_LAYOUT_STORAGE_KEY };

const STORAGE_KEYS: Record<DashboardMode, string> = {
  live: LIVE_LAYOUT_STORAGE_KEY,
  review: REVIEW_LAYOUT_STORAGE_KEY
};

@Injectable({ providedIn: 'root' })
export class LayoutPersistenceService {
  load(mode: DashboardMode): PersistedLayout | null {
    const raw = localStorage.getItem(STORAGE_KEYS[mode]);

    if (!raw) {
      return null;
    }

    try {
      const parsed = JSON.parse(raw) as unknown;
      return this.isPersistedLayout(parsed) ? parsed : null;
    } catch (error) {
      console.warn('[dashboard-framework] Failed to parse persisted dashboard layout.', error);
      return null;
    }
  }

  save(mode: DashboardMode, layout: PersistedLayout): void {
    localStorage.setItem(STORAGE_KEYS[mode], JSON.stringify(layout));
  }

  clear(mode: DashboardMode): void {
    localStorage.removeItem(STORAGE_KEYS[mode]);
  }

  private isPersistedLayout(value: unknown): value is PersistedLayout {
    if (!value || typeof value !== 'object') {
      return false;
    }

    const candidate = value as Partial<PersistedLayout>;
    return candidate.schemaVersion === 1 && Array.isArray(candidate.items);
  }
}
