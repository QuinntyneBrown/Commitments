import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'cui-dialog-shell',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dialog-shell.component.html',
  styleUrls: ['./dialog-shell.component.scss']
})
export class CuiDialogShellComponent {
  readonly title = input('');
  readonly cancelLabel = input('Cancel');
  readonly confirmLabel = input('Save');
  readonly showCancel = input(true);
  readonly showConfirm = input(true);
  readonly cancel = output<void>();
  readonly confirm = output<void>();
}
