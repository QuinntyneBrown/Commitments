import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'cui-sidenav-item',
  standalone: true,
  imports: [MatIconModule, MatListModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './sidenav-item.component.html',
  styleUrls: ['./sidenav-item.component.scss']
})
export class CuiSidenavItemComponent {
  readonly icon = input('');
  readonly label = input('');
  readonly active = input(false);
  readonly disabled = input(false);
  readonly selected = output<void>();
}
