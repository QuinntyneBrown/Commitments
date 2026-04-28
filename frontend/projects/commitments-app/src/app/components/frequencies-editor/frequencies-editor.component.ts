// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FrequencyType } from '../../models/frequency-type';
import { Frequency } from '../../models/frequency';
import { ColDef } from 'ag-grid-community';
import { DeleteCellComponent } from '../delete-cell/delete-cell.component';

@Component({
  selector: 'app-frequencies-editor',
  standalone: true,
  imports: [CommonModule, DeleteCellComponent],
  templateUrl: './frequencies-editor.component.html',
  styleUrls: ['./frequencies-editor.component.scss']
})
export class FrequenciesEditorComponent {
  public readonly frequencyTypes = input<Array<FrequencyType>>([]);
  public readonly frequencies = input<Array<Frequency>>([]);

  private _local: Array<Frequency> = [];

  ngOnInit() {
    this._local = [...(this.frequencies() ?? [])];
  }

  public get rows(): Array<Frequency> {
    return this._local;
  }

  public handleFrequencySave($event) {
    this._local = [...this._local, $event.frequency];
  }

  public onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public handleSaveClick() {}

  public remove($event) {
    this._local = this._local.filter(
      x => !(x.frequency == $event.data.frequency && x.frequencyTypeId == $event.data.frequencyTypeId)
    );
  }

  public columnDefs: Array<ColDef> = [
    {
      headerName: 'Frequency',
      field: 'frequency'
    },
    {
      headerName: 'Frequency Type',
      field: 'frequencyTypeId'
    },
    {
      cellRenderer: DeleteCellComponent,
      onCellClicked: frequency => this.remove(frequency)
    }
  ];
}
