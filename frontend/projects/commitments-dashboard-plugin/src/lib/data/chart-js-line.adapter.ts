// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ElementRef, Injectable } from '@angular/core';
import {
  Chart,
  ChartConfiguration,
  ChartDataset,
  Filler,
  LineController,
  LineElement,
  LinearScale,
  CategoryScale,
  PointElement,
  Tooltip
} from 'chart.js';

Chart.register(LineController, LineElement, PointElement, LinearScale, CategoryScale, Filler, Tooltip);
Chart.defaults.font.family = 'Inter, Roboto, "Helvetica Neue", sans-serif';
Chart.defaults.font.size = 11;

@Injectable()
export class ChartJsLineAdapter {
  private _chart: Chart<'line'> | null = null;

  attach(canvas: ElementRef<HTMLCanvasElement>, config: ChartConfiguration<'line'>): void {
    this.destroy();
    this._chart = new Chart(canvas.nativeElement, config);
  }

  updateDataset(dataset: ChartDataset<'line'>, labels: string[]): void {
    if (!this._chart) return;
    this._chart.data.labels = labels;
    this._chart.data.datasets = [dataset];
    this._chart.update('none');
  }

  destroy(): void {
    if (this._chart) {
      this._chart.destroy();
      this._chart = null;
    }
  }
}
