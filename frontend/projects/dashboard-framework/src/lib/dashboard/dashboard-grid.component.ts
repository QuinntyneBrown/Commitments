import {
  ChangeDetectionStrategy,
  Component,
  Injector,
  computed,
  inject,
  signal
} from '@angular/core';
import { NgComponentOutlet } from '@angular/common';
import { Gridster, GridsterItem as GridsterItemComponent, type GridsterItemConfig } from 'angular-gridster2';
import { DashboardItem } from './dashboard.model';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { TILE_CONTEXT, TileContext, TileRegistryService } from '../tile-registration';

@Component({
  selector: 'commitments-dashboard-grid',
  standalone: true,
  imports: [Gridster, GridsterItemComponent, NgComponentOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="dashboard-grid">
      @if (isEmpty()) {
        <div class="dashboard-grid__empty">
          <h2>No tiles on this dashboard</h2>
          <p>Use Add Tile to place a registered plugin tile.</p>
        </div>
      } @else {
        <gridster [options]="gridsterOptions()">
          @for (item of items(); track item.instanceId) {
            <gridster-item [item]="item" [attr.data-tile-id]="item.tileId">
              @if (layoutStore.isEditMode()) {
                <div class="tile-chrome">
                  <span class="tile-chrome__label">{{ registry.getTile(item.tileId)?.displayName ?? item.tileId }}</span>
                  <button
                    type="button"
                    class="tile-chrome__button"
                    (click)="removeTile(item.instanceId)"
                    aria-label="Remove tile"
                  >
                    x
                  </button>
                </div>
              }

              @if (registry.getTile(item.tileId); as descriptor) {
                <ng-container *ngComponentOutlet="descriptor.componentType; injector: tileInjector(item)"></ng-container>
              } @else {
                <span class="dashboard-grid__missing">{{ item.tileId }}</span>
              }
            </gridster-item>
          }
        </gridster>
      }
    </section>
  `,
  styles: [
    `
      :host {
        display: block;
        flex: 1 1 auto;
        min-height: 0;
      }

      .dashboard-grid {
        position: relative;
        display: block;
        height: 100%;
        min-height: 0;
        background: #f4f7fb;
      }

      gridster {
        width: 100%;
        height: 100%;
        background: transparent;
      }

      gridster-item {
        overflow: hidden;
        padding: 16px;
        border: 1px solid #d9e2ee;
        border-radius: 8px;
        background: #ffffff;
        box-shadow: 0 10px 24px rgba(28, 43, 66, 0.08);
      }

      .tile-chrome {
        position: absolute;
        top: 8px;
        right: 8px;
        left: 8px;
        z-index: 2;
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 8px;
        pointer-events: none;
      }

      .tile-chrome__label {
        overflow: hidden;
        padding: 3px 8px;
        border: 1px solid #d9e2ee;
        border-radius: 999px;
        color: #4a5b73;
        background: rgba(255, 255, 255, 0.92);
        font-size: 11px;
        font-weight: 700;
        text-overflow: ellipsis;
        white-space: nowrap;
      }

      .tile-chrome__button {
        width: 24px;
        height: 24px;
        border: 1px solid #d5dde8;
        border-radius: 6px;
        color: #344258;
        background: rgba(255, 255, 255, 0.95);
        cursor: pointer;
        font-size: 14px;
        line-height: 1;
        pointer-events: auto;
      }

      .tile-chrome__button:hover {
        color: #ffffff;
        border-color: #bd2b2b;
        background: #bd2b2b;
      }

      .dashboard-grid__empty {
        display: grid;
        place-items: center;
        align-content: center;
        height: 100%;
        color: #516178;
        text-align: center;
      }

      .dashboard-grid__empty h2 {
        margin: 0 0 6px;
        color: #1d2a3d;
        font-size: 20px;
      }

      .dashboard-grid__empty p {
        margin: 0;
      }
    `
  ]
})
export class DashboardGridComponent {
  private readonly parentInjector = inject(Injector);
  private readonly tileInjectors = new Map<string, Injector>();

  protected readonly layoutStore = inject(DashboardLayoutStore);
  protected readonly registry = inject(TileRegistryService);
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

    const maximizedSignal = signal(item.maximized);
    const context: TileContext = {
      tileId: item.tileId,
      instanceId: item.instanceId,
      isEditMode: this.layoutStore.isEditMode,
      isMaximized: maximizedSignal.asReadonly(),
      remove: () => this.layoutStore.removeTile(item.instanceId),
      maximize: () => {
        maximizedSignal.set(true);
        this.layoutStore.updateItem(item.instanceId, { maximized: true });
      },
      restore: () => {
        maximizedSignal.set(false);
        this.layoutStore.updateItem(item.instanceId, { maximized: false });
      },
      requestFocus: () => undefined
    };

    const injector = Injector.create({
      parent: this.parentInjector,
      providers: [{ provide: TILE_CONTEXT, useValue: context }]
    });
    this.tileInjectors.set(item.instanceId, injector);
    return injector;
  }
}
