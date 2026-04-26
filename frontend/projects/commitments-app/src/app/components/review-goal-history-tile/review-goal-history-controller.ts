// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Signal, computed, signal } from '@angular/core';

import { GoalProgress, GoalProgressService } from '../../services/goal-progress.service';

const MONTH_LABELS = [
  'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
  'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
];

function todayIsoDate(): string {
  return new Date().toISOString().slice(0, 10);
}

function endOfDayIso(date: string): string {
  return new Date(`${date}T23:59:59Z`).toISOString();
}

function formatDate(iso: string): string {
  const [y, m, d] = iso.split('-').map(Number);
  if (!y || !m || !d) return iso;
  return `${MONTH_LABELS[m - 1]} ${d}, ${y}`;
}

@Injectable()
export class ReviewGoalHistoryController {
  private _goalId: string | null = null;

  readonly historical = signal<GoalProgress | null>(null);
  readonly today = signal<GoalProgress | null>(null);

  readonly selectedDate = computed(() => this._selectedReviewDate() ?? todayIsoDate());
  readonly selectedDateLabel = computed(() => formatDate(this.selectedDate()));

  readonly count = computed(() => this.historical()?.count ?? 0);
  readonly target = computed(() => this.historical()?.target ?? 0);
  readonly delta = computed(() => (this.historical()?.count ?? 0) - (this.today()?.count ?? 0));
  readonly isEmpty = computed(() => (this.historical()?.count ?? 0) === 0);

  constructor(
    private readonly _service: GoalProgressService,
    private readonly _selectedReviewDate: Signal<string | null>
  ) {}

  load(goalId: string): void {
    this._goalId = goalId;
    this._fetchHistorical();
    this._service.getCurrent(goalId).subscribe(progress => this.today.set(progress));
  }

  refresh(): void {
    if (this._goalId) this._fetchHistorical();
  }

  private _fetchHistorical(): void {
    if (!this._goalId) return;
    const asOf = new Date(endOfDayIso(this.selectedDate()));
    this._service.getAt(this._goalId, asOf).subscribe(progress => this.historical.set(progress));
  }
}
