import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { DashboardGridComponent } from './dashboard-grid.component';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { TileRegistryService } from '../tile-registration';

@Component({
  selector: 'commitments-dashboard-shell',
  standalone: true,
  imports: [DashboardGridComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="dashboard-shell" data-testid="dashboard-shell">
      <header class="dashboard-shell__topbar">
        <div class="dashboard-shell__brand">
          <span class="dashboard-shell__eyebrow">Commitments</span>
          <h1>Dashboard</h1>
        </div>

        <div class="dashboard-shell__actions">
          <select
            class="dashboard-shell__select"
            data-testid="tile-select"
            [value]="selectedTileId()"
            (change)="selectedTileId.set($any($event.target).value)"
            aria-label="Tile to add"
          >
            @for (tile of tiles(); track tile.tileId) {
              <option [value]="tile.tileId">{{ tile.displayName }}</option>
            }
          </select>
          <button type="button" class="dashboard-shell__button" data-testid="add-tile" (click)="addSelectedTile()">Add Tile</button>
          <button
            type="button"
            class="dashboard-shell__button"
            data-testid="edit-layout"
            [class.dashboard-shell__button--active]="layoutStore.isEditMode()"
            (click)="layoutStore.toggleEditMode()"
          >
            {{ layoutStore.isEditMode() ? 'Done' : 'Edit Layout' }}
          </button>
          <button type="button" class="dashboard-shell__button dashboard-shell__button--muted" data-testid="reset-layout" (click)="layoutStore.resetLayout()">
            Reset
          </button>
        </div>
      </header>

      <main class="dashboard-shell__main">
        <commitments-dashboard-grid></commitments-dashboard-grid>
      </main>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        color: #172033;
        background: #f4f7fb;
        font-family: Inter, Roboto, "Helvetica Neue", Arial, sans-serif;
      }

      .dashboard-shell {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
      }

      .dashboard-shell__topbar {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 20px;
        min-height: 72px;
        padding: 12px 20px;
        border-bottom: 1px solid #dce5ef;
        background: #ffffff;
      }

      .dashboard-shell__brand {
        display: grid;
        gap: 2px;
      }

      .dashboard-shell__eyebrow {
        color: #65758d;
        font-size: 12px;
        font-weight: 700;
        text-transform: uppercase;
      }

      h1 {
        margin: 0;
        color: #172033;
        font-size: 24px;
        line-height: 1.1;
      }

      .dashboard-shell__actions {
        display: flex;
        align-items: center;
        gap: 8px;
      }

      .dashboard-shell__select,
      .dashboard-shell__button {
        height: 36px;
        border: 1px solid #c9d4e2;
        border-radius: 6px;
        background: #ffffff;
        color: #172033;
        font: inherit;
        font-size: 13px;
      }

      .dashboard-shell__select {
        min-width: 190px;
        padding: 0 32px 0 10px;
      }

      .dashboard-shell__button {
        padding: 0 12px;
        cursor: pointer;
        font-weight: 700;
      }

      .dashboard-shell__button:hover {
        border-color: #2f6db3;
        color: #1f5d9f;
      }

      .dashboard-shell__button--active {
        border-color: #235f9f;
        color: #ffffff;
        background: #235f9f;
      }

      .dashboard-shell__button--muted {
        color: #5b6b84;
      }

      .dashboard-shell__main {
        display: flex;
        flex: 1 1 auto;
        min-height: calc(100vh - 72px);
      }

      @media (max-width: 760px) {
        .dashboard-shell__topbar {
          align-items: stretch;
          flex-direction: column;
        }

        .dashboard-shell__actions {
          flex-wrap: wrap;
        }

        .dashboard-shell__select {
          flex: 1 1 180px;
        }
      }
    `
  ]
})
export class DashboardShellComponent {
  protected readonly layoutStore = inject(DashboardLayoutStore);
  private readonly registry = inject(TileRegistryService);

  protected readonly tiles = this.registry.tiles;
  protected readonly selectedTileId = signal('');

  constructor() {
    effect(() => {
      const firstTileId = this.tiles()[0]?.tileId ?? '';

      if (!this.selectedTileId() && firstTileId) {
        this.selectedTileId.set(firstTileId);
      }
    });
  }

  protected addSelectedTile(): void {
    const tileId = this.selectedTileId();

    if (tileId) {
      this.layoutStore.addTile(tileId);
    }
  }
}
