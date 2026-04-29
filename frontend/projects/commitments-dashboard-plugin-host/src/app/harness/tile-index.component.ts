import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TileRegistryService } from '@commitments/dashboard-framework';

@Component({
  selector: 'app-tile-index',
  standalone: true,
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <ul>
      @for (tile of registry.tiles(); track tile.tileId) {
        <li><a [routerLink]="['/tile', tile.tileId]">{{ tile.tileId }}</a></li>
      }
    </ul>
  `
})
export class TileIndexComponent {
  protected readonly registry = inject(TileRegistryService);
}
