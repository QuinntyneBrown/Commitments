import { Injectable } from '@angular/core';

export type HttpCall = { method: string; url: string; params?: Record<string, string> };
export type ChartCall =
  | { kind: 'attach'; type: 'line'; data: unknown; options: unknown }
  | { kind: 'updateDataset'; labels: string[]; dataset: unknown }
  | { kind: 'destroy' };

export interface PluginHarnessSnapshot {
  tileId: string | null;
  mode: 'live' | 'review';
  asOf: string | null;
  http: HttpCall[];
  chart: ChartCall[];
}

@Injectable({ providedIn: 'root' })
export class WindowBridgeService {
  private readonly snapshot: PluginHarnessSnapshot = {
    tileId: null, mode: 'live', asOf: null, http: [], chart: []
  };

  constructor() {
    window.__pluginHarness = this.snapshot;
  }

  setTile(tileId: string): void { this.snapshot.tileId = tileId; }
  setMode(mode: 'live' | 'review'): void { this.snapshot.mode = mode; }
  setAsOf(asOf: string | null): void { this.snapshot.asOf = asOf; }

  recordHttp(call: HttpCall): void { this.snapshot.http.push(call); }
  recordChart(call: ChartCall): void { this.snapshot.chart.push(call); }

  reset(): void {
    this.snapshot.tileId = null;
    this.snapshot.mode = 'live';
    this.snapshot.asOf = null;
    this.snapshot.http.length = 0;
    this.snapshot.chart.length = 0;
  }
}
