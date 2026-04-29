// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Signal, computed, signal } from '@angular/core';
import { ChartDataset, ScriptableContext } from 'chart.js';
import { ACCENT_CHART, BG_APP } from '@commitments/ui';
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
  readonly deltaPercentage = computed(() => {
    const match = this.deltaLabel().match(/-?\d+/);
    return match ? Number(match[0]) : 0;
  });
  readonly deltaCaption = computed(() => {
    const idx = this.deltaLabel().indexOf('%');
    return idx >= 0 ? this.deltaLabel().slice(idx + 1).trim() : '';
  });

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
      borderCapStyle: 'round',
      borderJoinStyle: 'round',
      tension: TENSION,
      fill: true,
      backgroundColor: (ctx: ScriptableContext<'line'>) => {
        const area = ctx.chart?.chartArea;
        if (!area) return ACCENT_CHART + '00';
        const gradient = ctx.chart.ctx.createLinearGradient(0, area.top, 0, area.bottom);
        gradient.addColorStop(0, ACCENT_CHART + '66');
        gradient.addColorStop(1, ACCENT_CHART + '00');
        return gradient;
      },
      pointRadius: points.map((_, i) => (i === highlighted ? POINT_RADIUS_HIGHLIGHT : POINT_RADIUS_DEFAULT)),
      pointBorderColor: points.map((_, i) => (i === highlighted ? BG_APP : 'transparent')),
      pointBorderWidth: points.map((_, i) => (i === highlighted ? 3 : 0)),
      pointBackgroundColor: ACCENT_CHART
    };
  });

  constructor(
    private readonly _service: GoalTrendService,
    private readonly _mode: Signal<DashboardMode>,
    private readonly _selectedReviewDate: Signal<string | null>
  ) {}

  load(goalId: string, windowDays: number = 30): Promise<void> {
    this._goalId = goalId;
    this._windowDays = windowDays;
    return this._fetch();
  }

  private async _fetch(): Promise<void> {
    if (!this._goalId) return;
    if (this._goalId === 'demo-goal') {
      this.trend.set(synthesizeDemoTrend(this._windowDays, this._mode(), this._selectedReviewDate()));
      return;
    }
    this.trend.set(await this._service.getTrend(this._goalId, this._mode(), this._selectedReviewDate(), this._windowDays));
  }
}

function synthesizeDemoTrend(
  windowDays: number,
  mode: DashboardMode,
  selectedReviewDate: string | null
): GoalTrendDto {
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  const points = Array.from({ length: windowDays }, (_, i) => {
    const date = new Date(today);
    date.setDate(today.getDate() - (windowDays - 1 - i));
    const wave = Math.sin(i / 3) * 12;
    const drift = i * 1.4;
    const noise = ((i * 7919) % 11) - 5;
    const percentage = Math.max(8, Math.min(96, Math.round(40 + drift + wave + noise)));
    return {
      date: date.toISOString().slice(0, 10),
      completed: Math.round((percentage / 100) * 30),
      target: 30,
      percentage
    };
  });
  const current = points[points.length - 1].percentage;
  const peak = Math.max(...points.map(p => p.percentage));
  const low = Math.min(...points.map(p => p.percentage));
  const prior = points.length >= 14
    ? Math.round(points.slice(0, points.length - 14).reduce((s, p) => s + p.percentage, 0) / Math.max(1, points.length - 14))
    : 0;
  const delta = current - prior;
  const sign = delta > 0 ? '+' : delta < 0 ? '' : '±';
  return {
    goalId: 'demo-goal',
    mode,
    asOf: selectedReviewDate ?? points[points.length - 1].date,
    windowDays,
    points,
    currentPercentage: current,
    peakPercentage: peak,
    lowPercentage: low,
    deltaLabel: `${sign}${delta}% vs prior 14d`
  };
}
