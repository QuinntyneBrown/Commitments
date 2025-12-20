// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { takeUntil, tap, map } from "rxjs";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { FrequencyService } from "./frequency.service";
import { Frequency } from "./frequency";
import { deepCopy } from "../core/deep-copy";
import { GridApi, ColDef } from "ag-grid";
import { CheckboxCellComponent } from "../shared/checkbox-cell.component";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";
import { EditFrequencyDialogService } from "./edit-frequency-dialog";

@Component({
  templateUrl: "./frequencies-page.component.html",
  styleUrls: ["./frequencies-page.component.scss"],
  selector: "app-frequencies-page"
})
export class FrequenciesPageComponent { 
  constructor(
    private readonly _editFrequencyDialog: EditFrequencyDialogService,
    private readonly _frequencyService: FrequencyService,
    private readonly _router: Router
  ) { }

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
      .pipe(takeUntil(this.onDestroy),map(x => this.addOrUpdate(x)))
      .subscribe();
  }

  public addOrUpdate(frequency: Frequency) {
    if (!frequency) return;

    let frequencies = [...this.frequencies$.value];
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

  private readonly _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }
}

