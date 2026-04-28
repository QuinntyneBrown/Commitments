import type { GridsterConfig } from 'angular-gridster2';

export interface DashboardItem {
  instanceId: string;
  tileId: string;
  cols: number;
  rows: number;
  x: number;
  y: number;
}

export interface PersistedLayout {
  schemaVersion: 1;
  savedAt: number;
  items: DashboardItem[];
}

export type { GridsterConfig };

export const DEFAULT_COLS = 12;
export const DEFAULT_ROWS = 8;

export const LIVE_LAYOUT_STORAGE_KEY = 'commitments.layout.live';
export const REVIEW_LAYOUT_STORAGE_KEY = 'commitments.layout.review';
