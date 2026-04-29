// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, TemplateRef, ViewChild, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FrequencyType } from '../../models/frequency-type';
import { Frequency } from '../../models/frequency';
import { FrequencyEditorComponent } from '../frequency-editor/frequency-editor.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DataTableColumn, DataTableComponent } from '@commitments/ui';

type FrequencyTableEvent = { data: Frequency };

@Component({
  selector: 'app-frequencies-editor',
  standalone: true,
  imports: [
    CommonModule,
    FrequencyEditorComponent,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
  ],
  templateUrl: './frequencies-editor.component.html',
  styleUrls: ['./frequencies-editor.component.scss'],
})
export class FrequenciesEditorComponent {
  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Frequency }>;

  public readonly frequencyTypes = input<Array<FrequencyType>>([]);
  public readonly frequencies = input<Array<Frequency>>([]);

  public columns: DataTableColumn<Frequency>[] = [];

  private _local: Array<Frequency> = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'frequency', header: 'Frequency', cell: (row) => `${row.frequency}` },
      { key: 'frequencyTypeId', header: 'Frequency Type', cell: (row) => `${row.frequencyTypeId}` },
      { key: 'delete', header: '', template: this.deleteTpl, width: '40px' },
    ];

    this._local = [...(this.frequencies() ?? [])];
  }

  public get rows(): Array<Frequency> {
    return this._local;
  }

  public handleFrequencySave($event: { frequency: Frequency }): void {
    this._local = [...this._local, $event.frequency];
  }

  public remove($event: FrequencyTableEvent): void {
    this._local = this._local.filter(
      (x) =>
        !(x.frequency == $event.data.frequency && x.frequencyTypeId == $event.data.frequencyTypeId),
    );
  }
}
