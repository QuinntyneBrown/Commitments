import { Signal, Type } from '@angular/core';

export type DashboardMode = 'live' | 'review';

export interface TileSize {
  readonly cols: number;
  readonly rows: number;
}

export interface TilePosition {
  readonly x: number;
  readonly y: number;
}

export interface TileMetadata {
  readonly tileId: string;
  readonly displayName: string;
  readonly description?: string;
  readonly category?: string;
  readonly defaultSize?: TileSize;
  readonly defaultPosition?: TilePosition;
  readonly includeByDefault?: boolean;
  readonly icon?: string;
}

export interface TileDescriptor extends TileMetadata {
  readonly componentType: Type<unknown>;
}

export interface TileContext {
  readonly tileId: string;
  readonly instanceId: string;
  readonly isEditMode: Signal<boolean>;
  readonly isMaximized: Signal<boolean>;
  readonly mode: Signal<DashboardMode>;
  readonly selectedReviewDate: Signal<string | null>;
  remove(): void;
  maximize(): void;
  restore(): void;
  requestFocus(): void;
}
