// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';

import {
  GoalProgressDto,
  GoalProgressService
} from '../../data/goal-progress.service';

type DeltaTone = 'success' | 'warn' | 'neutral';

@Injectable()
export class GoalMetricsController {
  private _goalId = '';

  readonly mode = signal<DashboardMode>('live');
  readonly current = signal<GoalProgressDto | null>(null);
  readonly today = signal<GoalProgressDto | null>(null);

  readonly count = computed(() => this.current()?.count ?? 0);
  readonly target = computed(() => this.current()?.target ?? 0);
  readonly percentage = computed(() => {
    const t = this.target();
    return t > 0 ? Math.round((this.count() / t) * 100) : 0;
  });

  private readonly _reviewDelta = computed<number | null>(() => {
    if (this.mode() !== 'review') return null;
    const c = this.current();
    const today = this.today();
    if (!c || !today) return null;
    return c.count - today.count;
  });

  readonly deltaLabel = computed<string | null>(() => {
    const delta = this._reviewDelta();
    if (delta === null) return null;
    if (delta === 0) return '±0 vs today';
    return `${delta > 0 ? '+' : ''}${delta} vs today`;
  });

  // Goal-progress is a positive metric: more = better.
  readonly deltaTone = computed<DeltaTone>(() => {
    const delta = this._reviewDelta();
    if (delta === null || delta === 0) return 'neutral';
    return delta < 0 ? 'success' : 'warn';
  });

  constructor(private readonly _service: GoalProgressService) {}

  setGoalId(goalId: string): void {
    this._goalId = goalId;
  }

  async load(mode: DashboardMode, asOf: string | null): Promise<void> {
    this.mode.set(mode);
    if (!this._goalId) return;

    if (mode === 'live') {
      this.today.set(null);
      this.current.set(await this._service.getCurrent(this._goalId));
      return;
    }

    if (!asOf) {
      this.today.set(null);
      this.current.set(null);
      return;
    }
    const [historical, now] = await Promise.all([
      this._service.getAt(this._goalId, asOf),
      this._service.getCurrent(this._goalId)
    ]);
    this.current.set(historical);
    this.today.set(now);
  }
}
