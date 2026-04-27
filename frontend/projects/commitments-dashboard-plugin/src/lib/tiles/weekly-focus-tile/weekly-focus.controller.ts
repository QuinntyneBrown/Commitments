// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';

import { WeeklyFocusDto, WeeklyFocusService } from '../../data/weekly-focus.service';

const MONTH_NAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

function formatDay(yyyymmdd: string): string {
  const [, m, d] = yyyymmdd.split('-').map(Number);
  return `${MONTH_NAMES[m - 1]} ${d}`;
}

@Injectable()
export class WeeklyFocusController {
  readonly snapshot = signal<WeeklyFocusDto | null>(null);
  readonly mode = signal<DashboardMode>('live');

  readonly focusAreas = computed(() => this.snapshot()?.focusAreas ?? []);
  readonly weekLabel = computed(() => {
    const snap = this.snapshot();
    return snap ? `${formatDay(snap.weekStart)} – ${formatDay(snap.weekEnd)}` : '';
  });

  constructor(private readonly _service: WeeklyFocusService) {}

  load(mode: DashboardMode, asOf: string | null): void {
    this.mode.set(mode);
    this._service.get(asOf).subscribe((dto) => this.snapshot.set(dto));
  }
}
