import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'commitments-tile-shell',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tile-shell.component.html',
  styleUrls: ['./tile-shell.component.scss']
})
export class TileShellComponent {
  @Input() title = '';
  @Input() eyebrow = '';
  @Input() status = '';
}
