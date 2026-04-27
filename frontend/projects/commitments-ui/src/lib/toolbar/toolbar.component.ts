import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'cui-toolbar',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatToolbarModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class CuiToolbarComponent {
  readonly appName = input('Commitments');
  readonly userName = input('');
  readonly avatarText = input('');
  readonly menuAriaLabel = input('Open navigation');
  readonly showMenuButton = input(true);
  readonly menuClick = output<void>();
}
