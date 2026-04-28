import {
  ChangeDetectionStrategy,
  Component,
  Injector,
  computed,
  inject
} from '@angular/core';
import { NgComponentOutlet } from '@angular/common';
import { Gridster, GridsterItem as GridsterItemComponent, type GridsterItemConfig } from 'angular-gridster2';
import { DashboardItem } from '../dashboard.model';
import { DashboardLayoutStore } from '../dashboard-layout.store';
import { DashboardModeService } from '../dashboard-mode.service';
import { TILE_CONTEXT, TileContext, TileRegistryService } from '../../tile-registration';

@Component({
  selector: 'commitments-dashboard-grid',
  standalone: true,
  imports: [Gridster, GridsterItemComponent, NgComponentOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-grid.component.html',
  styleUrls: ['./dashboard-grid.component.scss']
})
export class DashboardGridComponent {
  private readonly parentInjector = inject(Injector);
  private readonly tileInjectors = new Map<string, Injector>();

  protected readonly layoutStore = inject(DashboardLayoutStore);
  protected readonly registry = inject(TileRegistryService);
  private readonly modeService = inject(DashboardModeService);
  protected readonly items = this.layoutStore.items;
  protected readonly isEmpty = computed(() => this.items().length === 0);
  protected readonly gridsterOptions = computed(() => ({
    ...this.layoutStore.gridOptions(),
    itemChangeCallback: (item: GridsterItemConfig) => {
      const dashboardItem = item as unknown as DashboardItem;
      this.layoutStore.updateItem(dashboardItem.instanceId, {
        cols: dashboardItem.cols,
        rows: dashboardItem.rows,
        x: dashboardItem.x,
        y: dashboardItem.y
      });
    }
  }));

  protected removeTile(instanceId: string): void {
    this.layoutStore.removeTile(instanceId);
    this.tileInjectors.delete(instanceId);
  }

  protected tileInjector(item: DashboardItem): Injector {
    const cached = this.tileInjectors.get(item.instanceId);

    if (cached) {
      return cached;
    }

    const context: TileContext = {
      mode: this.modeService.mode,
      selectedReviewDate: this.modeService.selectedReviewDate
    };

    const injector = Injector.create({
      parent: this.parentInjector,
      providers: [{ provide: TILE_CONTEXT, useValue: context }]
    });
    this.tileInjectors.set(item.instanceId, injector);
    return injector;
  }
}
