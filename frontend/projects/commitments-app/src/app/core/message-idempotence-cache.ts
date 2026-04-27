// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

export class MessageIdempotenceCache {
  private readonly _seen = new Set<string>();
  private readonly _order: string[] = [];

  constructor(private readonly _capacity: number = 200) {}

  seen(messageId: string | null | undefined): boolean {
    if (!messageId) return false;
    if (this._seen.has(messageId)) return true;

    this._seen.add(messageId);
    this._order.push(messageId);

    if (this._order.length > this._capacity) {
      const drop = this._order.shift()!;
      this._seen.delete(drop);
    }
    return false;
  }

  clear(): void {
    this._seen.clear();
    this._order.length = 0;
  }
}
