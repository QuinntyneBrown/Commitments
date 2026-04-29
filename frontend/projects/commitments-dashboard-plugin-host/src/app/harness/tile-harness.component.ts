import {
  ChangeDetectionStrategy,
  Component,
  Injector,
  computed,
  inject,
  signal
} from '@angular/core';
import { NgComponentOutlet } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import {
  TILE_CONTEXT,
  TileContext,
  TileRegistryService,
  DashboardMode
} from '@commitments/dashboard-framework';
import { WindowBridgeService } from './window-bridge.service';

@Component({
  selector: 'app-tile-harness',
  standalone: true,
  imports: [NgComponentOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tile-harness.component.html'
})
export class TileHarnessComponent {
  private readonly registry = inject(TileRegistryService);
  private readonly route = inject(ActivatedRoute);
  private readonly parentInjector = inject(Injector);
  private readonly bridge = inject(WindowBridgeService);

  readonly tileId = signal<string>('');
  readonly mode = signal<DashboardMode>('live');
  readonly asOf = signal<string | null>(null);
  readonly tileInputs = signal<Record<string, unknown>>({});

  readonly descriptor = computed(() => this.registry.getTile(this.tileId()));

  readonly tileInjector = computed(() => {
    const ctx: TileContext = {
      mode: this.mode,
      selectedReviewDate: this.asOf
    };
    return Injector.create({
      parent: this.parentInjector,
      providers: [{ provide: TILE_CONTEXT, useValue: ctx }]
    });
  });

  constructor() {
    this.route.paramMap.subscribe((p) => {
      const id = p.get('tileId') ?? '';
      this.bridge.reset();
      this.tileId.set(id);
      this.bridge.setTile(id);
    });
    this.route.queryParamMap.subscribe((q) => {
      const m = q.get('mode');
      const mode: DashboardMode = m === 'review' ? 'review' : 'live';
      this.mode.set(mode);
      this.asOf.set(q.get('asOf'));
      this.bridge.setMode(mode);
      this.bridge.setAsOf(q.get('asOf'));
      const inputs: Record<string, unknown> = {};
      for (const key of q.keys) {
        if (key === 'mode' || key === 'asOf') continue;
        const v = q.get(key)!;
        inputs[key] = v !== '' && !isNaN(Number(v)) ? Number(v) : v;
      }
      this.tileInputs.set(inputs);
    });
  }
}
