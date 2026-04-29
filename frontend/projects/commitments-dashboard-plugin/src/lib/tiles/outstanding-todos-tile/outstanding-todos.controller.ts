// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';

import { OutstandingTodosDto, OutstandingTodosService } from '../../data/outstanding-todos.service';

type DeltaTone = 'success' | 'warn' | 'neutral';

@Injectable()
export class OutstandingTodosController {
  readonly snapshot = signal<OutstandingTodosDto | null>(null);
  readonly mode = signal<DashboardMode>('live');

  readonly count = computed(() => this.snapshot()?.count ?? 0);

  private readonly _reviewDelta = computed<number | null>(() => {
    const snap = this.snapshot();
    if (!snap || this.mode() !== 'review' || snap.todayCount == null) return null;
    return snap.count - snap.todayCount;
  });

  readonly deltaLabel = computed<string | null>(() => {
    const delta = this._reviewDelta();
    if (delta === null) return null;
    if (delta === 0) return '±0 vs today';
    return `${delta > 0 ? '+' : ''}${delta} vs today`;
  });

  // Outstanding is an inverted metric: fewer today = better.
  readonly deltaTone = computed<DeltaTone>(() => {
    const delta = this._reviewDelta();
    if (delta === null || delta === 0) return 'neutral';
    return delta > 0 ? 'success' : 'warn';
  });

  constructor(private readonly _service: OutstandingTodosService) {}

  async load(mode: DashboardMode, asOf: string | null): Promise<void> {
    this.mode.set(mode);
    this.snapshot.set(await this._service.get(mode, asOf));
  }
}
