// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';

import { GoalProgress, GoalProgressService } from '../../services/goal-progress.service';

import { localInputToIso, scrubInstantFromPct } from './review-goal-history-time';

@Injectable()
export class ReviewGoalHistoryController {
  private _goalId: string | null = null;
  private _appliedStart: number | null = null;
  private _appliedEnd: number | null = null;

  readonly draftStart = signal<number | null>(null);
  readonly draftEnd = signal<number | null>(null);
  readonly scrubPct = signal(50);
  readonly count = signal(0);
  readonly target = signal(0);
  readonly asOf = signal<Date | null>(null);

  readonly canApply = computed(() => {
    const start = this.draftStart();
    const end = this.draftEnd();
    return start !== null && end !== null && start < end;
  });

  constructor(private readonly _service: GoalProgressService) {}

  load(goalId: string): void {
    this._goalId = goalId;
  }

  setDraftStart(input: string): void {
    this.draftStart.set(this._inputToEpoch(input));
  }

  setDraftEnd(input: string): void {
    this.draftEnd.set(this._inputToEpoch(input));
  }

  apply(): void {
    if (!this.canApply()) return;
    this._appliedStart = this.draftStart();
    this._appliedEnd = this.draftEnd();
    this.scrubPct.set(50);
    this._fetchAtCurrentScrub();
  }

  scrub(pct: number): void {
    this.scrubPct.set(Math.max(0, Math.min(100, pct)));
    this._fetchAtCurrentScrub();
  }

  private _fetchAtCurrentScrub(): void {
    if (!this._goalId || this._appliedStart === null || this._appliedEnd === null) return;
    const instant = scrubInstantFromPct(this._appliedStart, this._appliedEnd, this.scrubPct());
    this._service.getAt(this._goalId, new Date(instant)).subscribe(p => this._setProgress(p));
  }

  private _setProgress(progress: GoalProgress): void {
    this.count.set(progress.count);
    this.target.set(progress.target);
    this.asOf.set(progress.asOf ? new Date(progress.asOf) : null);
  }

  private _inputToEpoch(input: string): number | null {
    const iso = localInputToIso(input);
    return iso ? new Date(iso).getTime() : null;
  }
}
