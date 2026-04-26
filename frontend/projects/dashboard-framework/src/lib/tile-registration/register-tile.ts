import { Type } from '@angular/core';
import { TileDescriptor, TileMetadata } from './tile.model';

type TileComponent = Type<unknown> & { tileMetadata?: TileMetadata };

export function registerTile(component: TileComponent): TileDescriptor {
  const metadata = component.tileMetadata;

  if (!metadata?.tileId) {
    throw new Error(
      `registerTile: component ${component?.name ?? '<anonymous>'} is missing static tileMetadata.tileId.`
    );
  }

  return { ...metadata, componentType: component };
}
