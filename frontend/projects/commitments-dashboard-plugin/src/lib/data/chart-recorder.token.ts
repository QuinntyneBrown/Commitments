import { InjectionToken } from '@angular/core';
import { ChartConfiguration, ChartDataset } from 'chart.js';

export interface ChartRecorder {
  onAttach(config: ChartConfiguration<'line'>): void;
  onUpdateDataset(dataset: ChartDataset<'line'>, labels: string[]): void;
  onDestroy(): void;
}

export const NOOP_CHART_RECORDER: ChartRecorder = {
  onAttach() {},
  onUpdateDataset() {},
  onDestroy() {}
};

export const CHART_RECORDER = new InjectionToken<ChartRecorder>('CHART_RECORDER', {
  providedIn: 'root',
  factory: () => NOOP_CHART_RECORDER
});
