// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

const DAY_MS = 24 * 60 * 60 * 1000;
const MONTH_LABELS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

function parseIsoDate(iso: string): Date {
  const [y, m, d] = iso.split('-').map(Number);
  return new Date(Date.UTC(y, m - 1, d));
}

function formatIsoDate(date: Date): string {
  const y = date.getUTCFullYear();
  const m = String(date.getUTCMonth() + 1).padStart(2, '0');
  const d = String(date.getUTCDate()).padStart(2, '0');
  return `${y}-${m}-${d}`;
}

export function clampIndex(index: number, max: number): number {
  if (index < 0) return 0;
  if (index > max) return max;
  return index;
}

export function isoDateAtIndex(start: string, index: number): string {
  const startDate = parseIsoDate(start);
  return formatIsoDate(new Date(startDate.getTime() + index * DAY_MS));
}

export function indexOfDate(start: string, date: string): number {
  const startDate = parseIsoDate(start);
  const target = parseIsoDate(date);
  return Math.round((target.getTime() - startDate.getTime()) / DAY_MS);
}

const MIN_TICK_PX = 64;

export function tickLabels(start: string, count: number, slotWidthPx: number): string[] {
  if (count <= 0) return [];
  const maxLabels = Math.max(1, Math.floor(slotWidthPx / MIN_TICK_PX));
  const stride = Math.max(1, Math.ceil(count / maxLabels));
  const labels: string[] = [];
  for (let i = 0; i < count; i += stride) {
    const date = parseIsoDate(isoDateAtIndex(start, i));
    labels.push(`${MONTH_LABELS[date.getUTCMonth()]} ${date.getUTCDate()}`);
  }
  return labels;
}
