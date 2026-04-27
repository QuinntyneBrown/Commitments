import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'cui-chip',
  standalone: true,
  imports: [MatChipsModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './chip.component.html',
  styleUrls: ['./chip.component.scss']
})
export class CuiChipComponent {
  readonly label = input('');
  readonly selected = input(false);
  readonly removable = input(false);
  readonly disabled = input(false);
  readonly removed = output<void>();
}
