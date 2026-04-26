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

export function buildVerticalGradient(
  ctx: CanvasRenderingContext2D,
  fromColor: string,
  height: number = 240
): CanvasGradient {
  const gradient = ctx.createLinearGradient(0, 0, 0, height);
  gradient.addColorStop(0, fromColor);
  gradient.addColorStop(1, 'rgba(0,0,0,0)');
  return gradient;
}
