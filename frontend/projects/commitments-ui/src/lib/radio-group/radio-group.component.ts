import { ChangeDetectionStrategy, Component, input, model } from '@angular/core';
import { MatRadioModule } from '@angular/material/radio';

export interface CuiRadioOption {
  value: string | number;
  label: string;
  disabled?: boolean;
}

@Component({
  selector: 'cui-radio-group',
  standalone: true,
  imports: [MatRadioModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './radio-group.component.html',
  styleUrls: ['./radio-group.component.scss']
})
export class CuiRadioGroupComponent {
  readonly label = input('');
  readonly options = input<readonly CuiRadioOption[]>([]);
  readonly disabled = input(false);
  readonly value = model<string | number>('');
}
