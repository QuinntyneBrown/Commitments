import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'cui-sidenav',
  standalone: true,
  imports: [MatListModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class CuiSidenavComponent {
  readonly ariaLabel = input('Primary navigation');
}
