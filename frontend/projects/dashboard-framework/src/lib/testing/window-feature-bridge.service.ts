import { Injectable } from '@angular/core';

export interface BackendCall {
  method: 'get' | 'post' | 'put' | 'delete';
  path: string;
  params?: Record<string, unknown>;
  body?: unknown;
}

export interface FeatureHarnessSnapshot {
  feature: string | null;
  route: string | null;
  backendCalls: BackendCall[];
}

@Injectable({ providedIn: 'root' })
export class WindowFeatureBridgeService {
  private readonly snapshot: FeatureHarnessSnapshot = {
    feature: null,
    route: null,
    backendCalls: []
  };

  constructor() {
    (window as unknown as { __featureHarness: FeatureHarnessSnapshot }).__featureHarness = this.snapshot;
  }

  setFeature(feature: string): void { this.snapshot.feature = feature; }
  setRoute(route: string): void { this.snapshot.route = route; }
  recordBackend(call: BackendCall): void { this.snapshot.backendCalls.push(call); }

  reset(): void {
    this.snapshot.feature = null;
    this.snapshot.route = null;
    this.snapshot.backendCalls.length = 0;
  }
}
