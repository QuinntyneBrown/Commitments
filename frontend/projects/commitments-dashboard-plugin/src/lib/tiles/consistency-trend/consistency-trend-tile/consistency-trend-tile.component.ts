// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  ViewChild,
  computed,
  effect,
  inject,
  input,
  signal
} from '@angular/core';
import { ChartConfiguration, Plugin } from 'chart.js';
import { TILE_CONTEXT, TileContext, TileMetadata } from '@commitments/dashboard-framework';
import {
  ACCENT_CHART,
  DeltaBadgeComponent,
  MetricHeaderComponent,
  StatusPillComponent,
  TEXT_MUTED,
  TileShellComponent
} from '@commitments/ui';

import { ChartJsLineAdapter } from '../../../data/chart-js-line.adapter';
import { GoalTrendService } from '../../../data/goal-trend.service';
import { ConsistencyTrendController } from '../consistency-trend.controller';

@Component({
  selector: 'commitments-consistency-trend-tile',
  standalone: true,
  imports: [TileShellComponent, StatusPillComponent, MetricHeaderComponent, DeltaBadgeComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ChartJsLineAdapter],
  templateUrl: './consistency-trend-tile.component.html',
  styleUrls: ['./consistency-trend-tile.component.scss']
})
export class ConsistencyTrendTileComponent implements AfterViewInit, OnDestroy {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.consistency-trend',
    displayName: 'Consistency Trend',
    description: 'Goal completion rate over time, mode-aware highlighting.',
    icon: 'trending_up',
    category: 'Commitments',
    defaultSize: { cols: 6, rows: 5 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false,
    supportedModes: ['live', 'review']
  };

  readonly goalId = input('demo-goal');
  readonly windowDays = input(30);

  @ViewChild('plot') plotRef!: ElementRef<HTMLCanvasElement>;

  private readonly _adapter = inject(ChartJsLineAdapter);
  private readonly _trendService = inject(GoalTrendService);
  private readonly _tileContext = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
  private readonly _fallbackMode = signal<'live' | 'review'>('live').asReadonly();
  private readonly _fallbackDate = signal<string | null>(null).asReadonly();

  readonly controller = new ConsistencyTrendController(
    this._trendService,
    this._tileContext?.mode ?? this._fallbackMode,
    this._tileContext?.selectedReviewDate ?? this._fallbackDate
  );

  constructor() {
    effect(() => {
      const id = this.goalId();
      if (id) {
        this.controller.load(id, this.windowDays());
      }
    });

    effect(() => {
      const dataset = this.controller.chartDataset();
      const labels = this.controller.chartLabels();
      if (this.plotRef) {
        this._adapter.updateDataset(dataset, labels);
      }
    });

    if (this._tileContext) {
      effect(() => {
        this._tileContext!.mode();
        this._tileContext!.selectedReviewDate();
        this.controller.refresh();
      });
    }
  }

  ngAfterViewInit(): void {
    this._adapter.attach(this.plotRef, this._buildChartConfig(this.controller));
  }

  private _buildChartConfig(controller: ConsistencyTrendController): ChartConfiguration<'line'> {
    const TODAY_GLOW_RADIUS = 7; // matches dataset POINT_RADIUS_HIGHLIGHT
    const TODAY_GLOW_BLUR = 8;   // design todayDot effect.blur
    const isHighlighted = (ctx: { index?: number }) =>
      ctx.index === controller.highlightedIndex();
    const todayPointGlow: Plugin<'line'> = {
      id: 'todayPointGlow',
      afterDatasetDraw(chart) {
        const idx = controller.highlightedIndex();
        if (idx < 0) return;
        const point = chart.getDatasetMeta(0).data[idx];
        if (!point) return;
        const ctx = chart.ctx;
        ctx.save();
        ctx.shadowBlur = TODAY_GLOW_BLUR;
        ctx.shadowColor = ACCENT_CHART + 'CC';
        ctx.fillStyle = ACCENT_CHART;
        ctx.beginPath();
        ctx.arc(point.x, point.y, TODAY_GLOW_RADIUS, 0, Math.PI * 2);
        ctx.fill();
        ctx.restore();
      }
    };
    return {
      type: 'line',
      data: { labels: controller.chartLabels(), datasets: [controller.chartDataset()] },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } },
        scales: {
          x: {
            type: 'category',
            ticks: {
              color: (ctx) => isHighlighted(ctx) ? ACCENT_CHART : TEXT_MUTED,
              font: (ctx) => ({ weight: isHighlighted(ctx) ? 700 : 400 }),
              autoSkip: true,
              maxTicksLimit: 5,
              padding: 6,
              callback(value) {
                const label = this.getLabelForValue(value as number);
                const date = new Date(label + 'T00:00:00');
                if (Number.isNaN(date.getTime())) return label;
                return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
              }
            },
            grid: { display: false },
            border: { display: false }
          },
          y: { min: 0, max: 100, ticks: { display: false, maxTicksLimit: 5 }, grid: { color: 'rgba(255, 255, 255, 0.07)', drawTicks: false }, border: { display: false } }
        },
        interaction: { mode: 'nearest', intersect: false, axis: 'x' }
      },
      plugins: [todayPointGlow]
    };
  }

  ngOnDestroy(): void {
    this._adapter.destroy();
  }

  protected readonly mode = computed(() => this._tileContext?.mode?.() ?? 'live');
  protected readonly pillVariant = computed<'chart' | 'review'>(() =>
    this.mode() === 'review' ? 'review' : 'chart'
  );
  protected readonly statusLabel = computed(() => this.mode().toUpperCase());
}
