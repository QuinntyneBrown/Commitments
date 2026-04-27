export class MisconfiguredTileError extends Error {
  constructor(tileId: string, reason: string) {
    super(`Tile "${tileId}" is misconfigured: ${reason}`);
    this.name = 'MisconfiguredTileError';
  }
}
