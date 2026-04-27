import { ChangeDetectionStrategy, Component, computed, input, model } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

export type CuiTextInputType = 'text' | 'email' | 'password' | 'search' | 'tel' | 'url' | 'number';

@Component({
  selector: 'cui-text-input',
  standalone: true,
  imports: [MatFormFieldModule, MatIconModule, MatInputModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class CuiTextInputComponent {
  readonly label = input('');
  readonly placeholder = input('');
  readonly hint = input('');
  readonly error = input('');
  readonly prefixIcon = input('');
  readonly suffixIcon = input('');
  readonly type = input<CuiTextInputType>('text');
  readonly disabled = input(false);
  readonly value = model('');

  protected readonly hasError = computed(() => this.error().trim().length > 0);

  protected updateValue(event: Event): void {
    this.value.set((event.target as HTMLInputElement).value);
  }
}
