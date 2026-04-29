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

  readonly tileId = signal<string>('');
  readonly mode = signal<DashboardMode>('live');
  readonly asOf = signal<string | null>(null);

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
    this.route.paramMap.subscribe((p) => this.tileId.set(p.get('tileId') ?? ''));
    this.route.queryParamMap.subscribe((q) => {
      const m = q.get('mode');
      this.mode.set(m === 'review' ? 'review' : 'live');
      this.asOf.set(q.get('asOf'));
    });
  }
}
