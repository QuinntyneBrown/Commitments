import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface CuiTableCell {
  text: string;
  width?: string;
  align?: 'start' | 'center' | 'end';
}

export type CuiTableRowVariant = 'header' | 'data';

@Component({
  selector: 'cui-table-row',
  standalone: true,
  imports: [MatButtonModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './table-row.component.html',
  styleUrls: ['./table-row.component.scss']
})
export class CuiTableRowComponent {
  readonly cells = input<readonly CuiTableCell[]>([]);
  readonly variant = input<CuiTableRowVariant>('data');
  readonly showActions = input(false);
  readonly edit = output<void>();
  readonly remove = output<void>();

  protected roleForCell(): string {
    return this.variant() === 'header' ? 'columnheader' : 'cell';
  }

  protected flexFor(cell: CuiTableCell): string {
    return cell.width ? `0 0 ${cell.width}` : '1 1 0';
  }

  protected justifyFor(cell: CuiTableCell): string {
    if (cell.align === 'center') return 'center';
    if (cell.align === 'end') return 'flex-end';
    return 'flex-start';
  }
}
