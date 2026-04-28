import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

export type TileShellVariant = 'metric' | 'review' | 'chart';

@Component({
  selector: 'commitments-tile-shell',
  standalone: true,
  imports: [MatCardModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tile-shell.component.html',
  styleUrls: ['./tile-shell.component.scss']
})
export class TileShellComponent {
  readonly title = input('');
  readonly eyebrow = input('');
  readonly icon = input('');
  readonly variant = input<TileShellVariant>('metric');
}
