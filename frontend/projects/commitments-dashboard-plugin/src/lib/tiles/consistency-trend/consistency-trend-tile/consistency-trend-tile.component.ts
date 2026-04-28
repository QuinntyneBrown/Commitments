// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
  computed,
  effect,
  inject,
  input,
  signal
} from '@angular/core';
import { ChartConfiguration } from 'chart.js';
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
export class ConsistencyTrendTileComponent implements OnInit, AfterViewInit, OnDestroy {
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

  ngOnInit(): void {
    const id = this.goalId();
    if (id) {
      this.controller.load(id, this.windowDays());
    }
  }

  ngAfterViewInit(): void {
    const controller = this.controller;
    const config: ChartConfiguration<'line'> = {
      type: 'line',
      data: { labels: this.controller.chartLabels(), datasets: [this.controller.chartDataset()] },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } },
        scales: {
          x: {
            type: 'category',
            ticks: {
              color: (ctx) => ctx.index === controller.highlightedIndex() ? ACCENT_CHART : TEXT_MUTED,
              font: (ctx) => ({ weight: ctx.index === controller.highlightedIndex() ? 700 : 400 }),
              autoSkip: true,
              maxTicksLimit: 6,
              callback(value) {
                const label = this.getLabelForValue(value as number);
                const date = new Date(label + 'T00:00:00');
                if (Number.isNaN(date.getTime())) return label;
                return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
              }
            },
            grid: { display: false }
          },
          y: { min: 0, max: 100, ticks: { display: false, maxTicksLimit: 5 }, grid: { color: 'rgba(255, 255, 255, 0.07)', drawTicks: false } }
        },
        interaction: { mode: 'nearest', intersect: false, axis: 'x' }
      }
    };
    this._adapter.attach(this.plotRef, config);
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
