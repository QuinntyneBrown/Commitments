// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';

import { HubClient } from '../../core/hub-client';
import { GoalProgressService } from '../../services/goal-progress.service';

interface GoalProgressUpdatedMessage {
  event: 'goalProgressUpdated';
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

  readonly pct = computed(() => {
    const target = this.target();
    if (target <= 0) return 0;
    const value = (this.count() / target) * 100;
    return Math.max(0, Math.min(100, value));
  });

  constructor(
    private readonly _hub: HubClient,
    private readonly _service: GoalProgressService
  ) {
    this._hub.messages$.subscribe(raw => {
      const msg = raw as Partial<GoalProgressUpdatedMessage> | null;
      if (msg?.event === 'goalProgressUpdated' && msg.goalId === this._goalId) {
        this.count.set(msg.count ?? 0);
        this.asOf.set(msg.asOf ? new Date(msg.asOf) : null);
      }
    });
  }

  load(goalId: string): void {
    this._goalId = goalId;
    this._service.getCurrent(goalId).subscribe(progress => {
      this.count.set(progress.count);
      this.target.set(progress.target);
      this.asOf.set(progress.asOf ? new Date(progress.asOf) : null);
    });
  }
}
