// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

export function scrubInstantFromPct(start: number, end: number, pct: number): number {
  const clamped = Math.max(0, Math.min(100, pct));
  return start + (end - start) * (clamped / 100);
}

export function localInputToIso(value: string): string | null {
  if (!value) return null;
  const date = new Date(value);
  if (isNaN(date.getTime())) return null;
  return date.toISOString();
}
