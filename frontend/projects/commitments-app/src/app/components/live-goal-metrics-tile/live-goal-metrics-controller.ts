// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';

import { HubClient } from '../../core/hub-client';
import { GoalProgressService, Last14DayPoint } from '../../services/goal-progress.service';

export interface GoalProgressUpdatedPayload {
  goalId: string;
  count: number;
  asOf: string;
}

@Injectable()
export class LiveGoalMetricsController {
  private _goalId: string | null = null;

  readonly count = signal(0);
  readonly target = signal(0);
  readonly asOf = signal<Date | null>(null);
  readonly last14 = signal<Last14DayPoint[]>([]);

  readonly pct = computed(() => {
    const target = this.target();
    if (target <= 0) return 0;
    const value = (this.count() / target) * 100;
    return Math.max(0, Math.min(100, value));
  });

  readonly percentDisplay = computed(() => `${Math.round(this.pct())}%`);

  readonly deltaVsYesterday = computed(() => {
    const series = this.last14();
    if (series.length < 2) return 0;
    return series[series.length - 1].completed - series[series.length - 2].completed;
  });

  constructor(
    private readonly _hub: HubClient,
    private readonly _service: GoalProgressService
  ) {
    this._hub.on<GoalProgressUpdatedPayload>('goalProgressUpdated').subscribe(payload => {
      if (payload.goalId !== this._goalId) return;
      this.count.set(payload.count);
      this.asOf.set(new Date(payload.asOf));
      this._patchTodayInLast14(payload.count);
    });
  }

  load(goalId: string): void {
    this._goalId = goalId;
    this._service.getCurrent(goalId).subscribe(progress => {
      this.count.set(progress.count);
      this.target.set(progress.target);
      this.asOf.set(progress.asOf ? new Date(progress.asOf) : null);
    });
    this._service.getLast14(goalId).subscribe(points => this.last14.set(points));
  }

  private _patchTodayInLast14(completed: number): void {
    const series = this.last14();
    if (series.length === 0) return;
    const last = series[series.length - 1];
    const target = last.target || this.target() || 1;
    const updated: Last14DayPoint = {
      ...last,
      completed,
      percent: Math.round((completed / target) * 100)
    };
    this.last14.set([...series.slice(0, -1), updated]);
  }
}
