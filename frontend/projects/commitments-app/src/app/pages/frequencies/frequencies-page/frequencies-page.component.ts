// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
import { FrequencyService } from '../../../services/frequency.service';
import { Frequency } from '../../../models/frequency';
import { EditFrequencyDialogService } from '../../../services/edit-frequency-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-frequencies-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './frequencies-page.component.html',
  styleUrls: ['./frequencies-page.component.scss']
})
export class FrequenciesPageComponent {
  private readonly _editFrequencyDialog = inject(EditFrequencyDialogService);
  private readonly _frequencyService = inject(FrequencyService);
  private readonly _router = inject(Router);

  ngOnInit() {
    this._frequencyService.get()
      .pipe(map(x => this.frequencies$.next(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public frequencies$: BehaviorSubject<Array<Frequency>> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleRemoveClick($event) {
    const frequencies: Array<Frequency> = [...this.frequencies$.value];
    const index = frequencies.findIndex(x => x.frequencyId == $event.data.frequencyId);
    frequencies.splice(index, 1);
    this.frequencies$.next(frequencies);

    this._frequencyService.remove({ frequency: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {

  }

  public handleFABButtonClick() {
    this._editFrequencyDialog.create()
      .pipe(takeUntil(this.onDestroy), map(x => this.addOrUpdate(x)))
      .subscribe();
  }

  public addOrUpdate(frequency: Frequency) {
    if (!frequency) return;

    const frequencies = [...this.frequencies$.value];
    const i = frequencies.findIndex((t) => t.frequencyId == frequency.frequencyId);
    const _ = i < 0 ? frequencies.push(frequency) : frequencies[i] = frequency;
    this.frequencies$.next(frequencies);
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name" },
    { cellRenderer: "editRenderer", onCellClicked: $event => this.handleEditClick($event), width: 30 },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemoveClick($event), width: 30 }
  ];

  public frameworkComponents: any = {
    checkboxRenderer: CheckboxCellComponent,
    deleteRenderer: DeleteCellComponent,
    editRenderer: EditCellComponent
  };

  private _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }
}
