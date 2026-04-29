// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';

import { MonthlyProgressDto, MonthlyProgressService } from '../../data/monthly-progress.service';

@Injectable()
export class MonthlyProgressController {
  readonly snapshot = signal<MonthlyProgressDto | null>(null);
  readonly mode = signal<DashboardMode>('live');

  readonly buckets = computed(() => this.snapshot()?.buckets ?? []);
  readonly isEmpty = computed(() => this.snapshot()?.isEmpty ?? true);

  constructor(private readonly _service: MonthlyProgressService) {}

  async load(mode: DashboardMode, asOf: string | null): Promise<void> {
    this.mode.set(mode);
    this.snapshot.set(await this._service.get(asOf));
  }
}
