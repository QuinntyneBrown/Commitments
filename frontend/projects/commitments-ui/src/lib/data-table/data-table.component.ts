// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import {
  ChangeDetectionStrategy,
  Component,
  TemplateRef,
  TrackByFunction,
  computed,
  effect,
  input,
  output,
  viewChild,
} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

export interface DataTableColumn<T> {
  key: string;
  header: string;
  cell?: (row: T) => string;
  template?: TemplateRef<{ $implicit: T }>;
  width?: string;
}

function defaultTrackBy<T>(_index: number, row: T): T {
  return row;
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [MatPaginatorModule, MatSortModule, MatTableModule, NgTemplateOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.scss'],
})
export class DataTableComponent<T> {
  readonly rows = input<T[]>([]);
  readonly columns = input.required<DataTableColumn<T>[]>();
  readonly pageSize = input(5);
  readonly trackBy = input<TrackByFunction<T>>(defaultTrackBy);
  readonly rowClick = output<T>();

  readonly displayedColumns = computed(() => this.columns().map((column) => column.key));
  readonly dataSource = new MatTableDataSource<T>([]);

  private readonly paginator = viewChild(MatPaginator);

  constructor() {
    effect(() => {
      this.dataSource.data = this.rows();

      const paginator = this.paginator();
      if (paginator) {
        this.dataSource.paginator = paginator;
      }
    });
  }

  cellText(row: T, column: DataTableColumn<T>): string {
    if (column.cell) {
      return column.cell(row);
    }

    const value = (row as Record<string, unknown>)[column.key];
    return value == null ? '' : String(value);
  }
}
