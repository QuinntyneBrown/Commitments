import { Injectable, inject } from '@angular/core';
import { ChartConfiguration, ChartDataset } from 'chart.js';
import { ChartRecorder } from '@commitments/dashboard-plugin';
import { WindowBridgeService } from './window-bridge.service';

function safeJson(value: unknown): unknown {
  return JSON.parse(JSON.stringify(value, (_, v) => (typeof v === 'function' ? '<callback>' : v)));
}

@Injectable({ providedIn: 'root' })
export class WindowBridgeChartRecorder implements ChartRecorder {
  private readonly bridge = inject(WindowBridgeService);

  onAttach(config: ChartConfiguration<'line'>): void {
    this.bridge.recordChart({
      kind: 'attach',
      type: config.type,
      data: safeJson(config.data),
      options: safeJson(config.options)
    });
  }

  onUpdateDataset(dataset: ChartDataset<'line'>, labels: string[]): void {
    this.bridge.recordChart({
      kind: 'updateDataset',
      labels: [...labels],
      dataset: safeJson(dataset)
    });
  }

  onDestroy(): void {
    this.bridge.recordChart({ kind: 'destroy' });
  }
}
