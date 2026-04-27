import { ChangeDetectionStrategy, Component, input, model } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';

export type CuiSidenavMode = 'side' | 'over' | 'push';

@Component({
  selector: 'cui-app-shell',
  standalone: true,
  imports: [MatSidenavModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './app-shell.component.html',
  styleUrls: ['./app-shell.component.scss']
})
export class CuiAppShellComponent {
  readonly mode = input<CuiSidenavMode>('side');
  readonly opened = model(true);
}
