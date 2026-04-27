// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';

import { DailyResultsDto, DailyResultsService } from '../../data/daily-results.service';

@Injectable()
export class DailyResultsController {
  readonly snapshot = signal<DailyResultsDto | null>(null);
  readonly mode = signal<DashboardMode>('live');

  readonly completed = computed(() => this.snapshot()?.completed ?? 0);
  readonly total = computed(() => this.snapshot()?.total ?? 0);
  readonly percentage = computed(() => {
    const total = this.total();
    return total > 0 ? Math.round((this.completed() / total) * 100) : 0;
  });

  constructor(private readonly _service: DailyResultsService) {}

  load(mode: DashboardMode, asOf: string | null): void {
    this.mode.set(mode);
    this._service.get(asOf).subscribe((dto) => this.snapshot.set(dto));
  }
}
