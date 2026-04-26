// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
  effect,
  inject,
  signal
} from '@angular/core';
import { ChartConfiguration } from 'chart.js';
import { TILE_CONTEXT, TileContext, TileMetadata } from '@commitments/dashboard-framework';
import {
  DeltaBadgeComponent,
  MetricHeaderComponent,
  StatusPillComponent,
  TileShellComponent
} from '@commitments/ui';

import { ChartJsLineAdapter } from '../../data/chart-js-line.adapter';
import { GoalTrendService } from '../../data/goal-trend.service';
import { ConsistencyTrendController } from './consistency-trend.controller';

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
    category: 'Commitments',
    defaultSize: { cols: 6, rows: 4 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false,
    supportedModes: ['live', 'review']
  };

  @Input() goalId = '';
  @Input() windowDays = 30;

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
    if (this.goalId) {
      this.controller.load(this.goalId, this.windowDays);
    }
  }

  ngAfterViewInit(): void {
    const config: ChartConfiguration<'line'> = {
      type: 'line',
      data: { labels: this.controller.chartLabels(), datasets: [this.controller.chartDataset()] },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } },
        scales: {
          x: { type: 'category', ticks: { color: '#666666', autoSkip: true, maxTicksLimit: 6 }, grid: { display: false } },
          y: { min: 0, max: 100, ticks: { display: false }, grid: { color: '#3A3A3A', drawTicks: false } }
        },
        interaction: { mode: 'nearest', intersect: false, axis: 'x' }
      }
    };
    this._adapter.attach(this.plotRef, config);
  }

  ngOnDestroy(): void {
    this._adapter.destroy();
  }

  pillVariant(): 'live' | 'review' {
    return this._tileContext?.mode?.() ?? 'live';
  }

  pillLabel(): string {
    return this.pillVariant().toUpperCase();
  }
}
