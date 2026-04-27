import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export type CuiButtonAppearance = 'raised' | 'stroked' | 'basic' | 'icon' | 'fab' | 'mini-fab' | 'extended-fab';
export type CuiButtonTone = 'primary' | 'accent' | 'warn' | 'neutral';
export type CuiButtonType = 'button' | 'submit' | 'reset';

@Component({
  selector: 'cui-button',
  standalone: true,
  imports: [MatButtonModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class CuiButtonComponent {
  readonly appearance = input<CuiButtonAppearance>('raised');
  readonly tone = input<CuiButtonTone>('primary');
  readonly label = input('');
  readonly icon = input('');
  readonly ariaLabel = input('');
  readonly disabled = input(false);
  readonly type = input<CuiButtonType>('button');
  readonly clicked = output<MouseEvent>();

  protected readonly materialColor = computed(() => (this.tone() === 'neutral' ? undefined : this.tone()));

  protected emitClick(event: MouseEvent): void {
    if (this.disabled()) {
      event.preventDefault();
      return;
    }

    this.clicked.emit(event);
  }
}
