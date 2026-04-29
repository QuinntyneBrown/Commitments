import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatButtonModule } from '@angular/material/button';
import { By } from '@angular/platform-browser';

import { DataTableColumn, DataTableComponent } from './data-table.component';

interface TestRow {
  id: number;
  name: string;
  status?: string;
}

@Component({
  standalone: true,
  imports: [DataTableComponent, MatButtonModule],
  template: `
    <app-data-table
      [rows]="rows"
      [columns]="columns"
      [pageSize]="pageSize"
      (rowClick)="clicked = $event"
    ></app-data-table>

    <ng-template #actionTpl let-row>
      <button type="button" (click)="selected = row">Select {{ row.name }}</button>
    </ng-template>
  `,
})
class DataTableHostComponent implements OnInit {
  @ViewChild('actionTpl', { static: true })
  actionTpl!: TemplateRef<{ $implicit: TestRow }>;

  rows: TestRow[] = [
    { id: 1, name: 'Morning review', status: 'Ready' },
    { id: 2, name: 'Weekly plan', status: 'Draft' },
    { id: 3, name: 'Retrospective', status: 'Done' },
  ];

  columns: DataTableColumn<TestRow>[] = [];
  pageSize = 5;
  selected: TestRow | undefined;
  clicked: TestRow | undefined;

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name' },
      { key: 'state', header: 'State', cell: (row) => row.status ?? '' },
      { key: 'action', header: '', template: this.actionTpl, width: '50px' },
    ];
  }
}

describe('DataTableComponent', () => {
  let fixture: ComponentFixture<DataTableHostComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [DataTableHostComponent] });
    fixture = TestBed.createComponent(DataTableHostComponent);
    fixture.detectChanges();
  });

  it('renders configured header and text cells', () => {
    const text = (fixture.nativeElement as HTMLElement).textContent ?? '';

    expect(text).toContain('Name');
    expect(text).toContain('Morning review');
    expect(text).toContain('Ready');
  });

  it('renders custom cell templates with the row as implicit context', () => {
    const button = (fixture.nativeElement as HTMLElement).querySelector('button');
    button?.click();
    fixture.detectChanges();

    expect(fixture.componentInstance.selected?.name).toBe('Morning review');
  });

  it('emits rowClick when a table row is clicked', () => {
    const row = (fixture.nativeElement as HTMLElement).querySelector('tbody tr');
    row?.dispatchEvent(new MouseEvent('click', { bubbles: true }));

    expect(fixture.componentInstance.clicked?.name).toBe('Morning review');
  });

  it('defaults to a 5 row page size and identity trackBy', () => {
    const component = fixture.debugElement.query(By.directive(DataTableComponent))
      .componentInstance as DataTableComponent<TestRow>;
    const row = fixture.componentInstance.rows[0];

    expect(component.pageSize()).toBe(5);
    expect(component.trackBy()(0, row)).toBe(row);
  });
});
