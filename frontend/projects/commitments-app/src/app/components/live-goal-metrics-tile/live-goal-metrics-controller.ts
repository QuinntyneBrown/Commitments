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

function isGoalProgressUpdated(value: unknown): value is GoalProgressUpdatedMessage {
  const msg = value as GoalProgressUpdatedMessage | null;
  return !!msg && msg.event === 'goalProgressUpdated';
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
    this._hub.messages$.subscribe(message => {
      if (isGoalProgressUpdated(message) && message.goalId === this._goalId) {
        this.count.set(message.count);
        this.asOf.set(new Date(message.asOf));
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
