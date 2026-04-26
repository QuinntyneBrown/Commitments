import { Injectable, inject } from '@angular/core';
import { PLUGIN_TILES } from './plugin-tiles.token';
import { registerTile } from './register-tile';
import { TileRegistryService } from './tile-registry.service';

@Injectable({ providedIn: 'root' })
export class TileRegistrationBootstrapper {
  private readonly registry = inject(TileRegistryService);
  private readonly contributions = inject(PLUGIN_TILES, { optional: true }) ?? [];

  bootstrap(): void {
    for (const component of this.contributions.flat()) {
      try {
        this.registry.registerTile(registerTile(component));
      } catch (error) {
        console.warn(`[dashboard-framework] ${(error as Error).message}`);
      }
    }
  }
}
