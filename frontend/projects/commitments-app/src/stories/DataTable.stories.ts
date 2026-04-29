import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { expect, fn, userEvent, within } from '@storybook/test';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DataTableColumn, DataTableComponent } from '@commitments/ui';

interface StoryRow {
  id: number;
  name: string;
  owner: string;
}

@Component({
  selector: 'app-data-table-story',
  standalone: true,
  imports: [DataTableComponent, MatButtonModule, MatIconModule],
  template: `
    <app-data-table [rows]="rows" [columns]="columns" [pageSize]="5"></app-data-table>

    <ng-template #editActionTpl let-row>
      <button
        mat-icon-button
        type="button"
        [attr.aria-label]="'Edit ' + row.name"
        (click)="editRow.emit(row)"
      >
        <mat-icon>edit</mat-icon>
      </button>
    </ng-template>

    <ng-template #deleteActionTpl let-row>
      <button
        mat-icon-button
        type="button"
        [attr.aria-label]="'Delete ' + row.name"
        (click)="deleteRow.emit(row)"
      >
        <mat-icon>close</mat-icon>
      </button>
    </ng-template>
  `,
})
class DataTableStoryComponent implements OnInit {
  @Output() editRow = new EventEmitter<StoryRow>();
  @Output() deleteRow = new EventEmitter<StoryRow>();

  @ViewChild('editActionTpl', { static: true })
  editActionTpl!: TemplateRef<{ $implicit: StoryRow }>;

  @ViewChild('deleteActionTpl', { static: true })
  deleteActionTpl!: TemplateRef<{ $implicit: StoryRow }>;

  rows: StoryRow[] = [
    { id: 1, name: 'Morning review', owner: 'Alex' },
    { id: 2, name: 'Weekly plan', owner: 'Sam' },
    { id: 3, name: 'Retrospective', owner: 'Taylor' },
  ];

  columns: DataTableColumn<StoryRow>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (row) => row.name },
      { key: 'owner', header: 'Owner', cell: (row) => row.owner },
      { key: 'edit', header: '', template: this.editActionTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteActionTpl, width: '50px' },
    ];
  }
}

const meta: Meta = {
  title: 'Commitments UI/Data Table',
  decorators: [
    moduleMetadata({
      imports: [DataTableStoryComponent],
    }),
  ],
  parameters: {
    layout: 'padded',
  },
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<{ editRow: (row: StoryRow) => void; deleteRow: (row: StoryRow) => void }>;

export const Basic: Story = {
  args: {
    editRow: fn(),
    deleteRow: fn(),
  },
  render: (args) => ({
    props: args,
    template: `
      <app-data-table-story
        (editRow)="editRow($event)"
        (deleteRow)="deleteRow($event)"
      ></app-data-table-story>
    `,
  }),
  play: async ({ args, canvasElement }) => {
    const canvas = within(canvasElement);

    await userEvent.click(canvas.getByRole('button', { name: 'Edit Morning review' }));
    await userEvent.click(canvas.getByRole('button', { name: 'Delete Morning review' }));

    expect(args.editRow).toHaveBeenCalledWith(expect.objectContaining({ id: 1 }));
    expect(args.deleteRow).toHaveBeenCalledWith(expect.objectContaining({ id: 1 }));
  },
};
