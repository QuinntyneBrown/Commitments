// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Signal, computed, signal } from '@angular/core';
import { ChartDataset } from 'chart.js';
import { ACCENT_CHART, SURFACE_TILE } from '@commitments/ui';
import { DashboardMode } from '@commitments/dashboard-framework';

import { GoalTrendDto, GoalTrendService } from '../../data/goal-trend.service';

const POINT_RADIUS_DEFAULT = 3;
const POINT_RADIUS_HIGHLIGHT = 7;
const BORDER_WIDTH = 2.5;
const TENSION = 0.35;

@Injectable()
export class ConsistencyTrendController {
  private _goalId: string | null = null;
  private _windowDays = 30;

  readonly trend = signal<GoalTrendDto | null>(null);

  readonly currentPercentage = computed(() => this.trend()?.currentPercentage ?? 0);
  readonly peakPercentage = computed(() => this.trend()?.peakPercentage ?? 0);
  readonly lowPercentage = computed(() => this.trend()?.lowPercentage ?? 0);
  readonly deltaLabel = computed(() => this.trend()?.deltaLabel ?? '');

  readonly highlightedIndex = computed(() => {
    const trend = this.trend();
    if (!trend || trend.points.length === 0) return -1;
    if (this._mode() === 'live') return trend.points.length - 1;
    const date = this._selectedReviewDate();
    if (!date) return -1;
    return trend.points.findIndex(p => p.date === date);
  });

  readonly chartLabels = computed(() => this.trend()?.points.map(p => p.date) ?? []);

  readonly chartDataset = computed<ChartDataset<'line'>>(() => {
    const trend = this.trend();
    const points = trend?.points ?? [];
    const highlighted = this.highlightedIndex();
    return {
      data: points.map(p => p.percentage),
      borderColor: ACCENT_CHART,
      borderWidth: BORDER_WIDTH,
      tension: TENSION,
      fill: true,
      backgroundColor: ACCENT_CHART + '33',
      pointRadius: points.map((_, i) => (i === highlighted ? POINT_RADIUS_HIGHLIGHT : POINT_RADIUS_DEFAULT)),
      pointBorderColor: SURFACE_TILE,
      pointBackgroundColor: ACCENT_CHART
    };
  });

  constructor(
    private readonly _service: GoalTrendService,
    private readonly _mode: Signal<DashboardMode>,
    private readonly _selectedReviewDate: Signal<string | null>
  ) {}

  load(goalId: string, windowDays: number = 30): void {
    this._goalId = goalId;
    this._windowDays = windowDays;
    this._fetch();
  }

  refresh(): void {
    if (this._goalId) this._fetch();
  }

  private _fetch(): void {
    if (!this._goalId) return;
    this._service
      .getTrend(this._goalId, this._mode(), this._selectedReviewDate(), this._windowDays)
      .subscribe(trend => this.trend.set(trend));
  }
}
