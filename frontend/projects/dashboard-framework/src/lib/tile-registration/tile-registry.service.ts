import { Injectable, Signal, signal } from '@angular/core';
import { TileDescriptor } from './tile.model';

@Injectable({ providedIn: 'root' })
export class TileRegistryService {
  private readonly descriptors = signal<TileDescriptor[]>([]);
  private readonly byId = new Map<string, TileDescriptor>();

  readonly tiles: Signal<TileDescriptor[]> = this.descriptors.asReadonly();

  registerTile(descriptor: TileDescriptor): void {
    if (this.byId.has(descriptor.tileId)) {
      console.warn(
        `[dashboard-framework] Duplicate tile registration for "${descriptor.tileId}" ignored.`
      );
      return;
    }

    this.byId.set(descriptor.tileId, descriptor);
    this.descriptors.update((tiles) => [...tiles, descriptor]);
  }

  getTile(tileId: string): TileDescriptor | undefined {
    return this.byId.get(tileId);
  }

  listTiles(): TileDescriptor[] {
    return this.descriptors();
  }
}
