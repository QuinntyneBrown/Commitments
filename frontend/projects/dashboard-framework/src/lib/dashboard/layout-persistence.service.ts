import { Injectable } from '@angular/core';
import { LAYOUT_STORAGE_KEY, PersistedLayout } from './dashboard.model';

@Injectable({ providedIn: 'root' })
export class LayoutPersistenceService {
  load(): PersistedLayout | null {
    const raw = localStorage.getItem(LAYOUT_STORAGE_KEY);

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

  save(layout: PersistedLayout): void {
    localStorage.setItem(LAYOUT_STORAGE_KEY, JSON.stringify(layout));
  }

  clear(): void {
    localStorage.removeItem(LAYOUT_STORAGE_KEY);
  }

  private isPersistedLayout(value: unknown): value is PersistedLayout {
    if (!value || typeof value !== 'object') {
      return false;
    }

    const candidate = value as Partial<PersistedLayout>;
    return candidate.schemaVersion === 1 && Array.isArray(candidate.items);
  }
}
