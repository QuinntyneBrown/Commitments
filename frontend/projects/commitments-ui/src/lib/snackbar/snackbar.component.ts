import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export type CuiSnackbarTone = 'neutral' | 'error' | 'success';

@Component({
  selector: 'cui-snackbar',
  standalone: true,
  imports: [MatButtonModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.scss']
})
export class CuiSnackbarComponent {
  readonly message = input('');
  readonly action = input('');
  readonly tone = input<CuiSnackbarTone>('neutral');
  readonly actionClick = output<void>();

  protected readonly icon = computed(() => {
    if (this.tone() === 'error') return 'error';
    if (this.tone() === 'success') return 'check_circle';
    return '';
  });

  protected readonly role = computed(() => (this.tone() === 'error' ? 'alert' : 'status'));
}
