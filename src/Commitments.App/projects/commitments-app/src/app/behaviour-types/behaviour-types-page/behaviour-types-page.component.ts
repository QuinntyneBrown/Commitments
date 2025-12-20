// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { takeUntil, tap, map } from "rxjs";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { BehaviourTypeService } from "./behaviour-type.service";
import { BehaviourType } from "./behaviour-type";
import { deepCopy } from "../core/deep-copy";
import { GridApi, ColDef } from "ag-grid";
import { CheckboxCellComponent } from "../shared/checkbox-cell.component";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";
import { EditBehaviourTypeDialogService } from "./edit-behaviour-type-dialog";

@Component({
  templateUrl: "./behaviour-types-page.component.html",
  styleUrls: ["./behaviour-types-page.component.scss"],
  selector: "app-behaviour-types-page"
})
export class BehaviourTypesPageComponent { 
  constructor(
    private readonly _behaviourTypeService: BehaviourTypeService,
    private readonly _editBehaviourTypeDialog: EditBehaviourTypeDialogService,
    private readonly _router: Router
  ) { }

  ngOnInit() {
    this._behaviourTypeService.get()
      .pipe(map(x => this.behaviourTypes$.next(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public behaviourTypes$: BehaviorSubject<Array<BehaviourType>> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  public handleRemoveClick($event) {
    const behaviourTypes: Array<BehaviourType> = [...this.behaviourTypes$.value];
    const index = behaviourTypes.findIndex(x => x.behaviourTypeId == $event.data.behaviourTypeId);
    behaviourTypes.splice(index, 1);
    this.behaviourTypes$.next(behaviourTypes);

    this._behaviourTypeService.remove({ behaviourType: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {    

  }

  public handleFABButtonClick() {
    this._editBehaviourTypeDialog.create()
      .pipe(takeUntil(this.onDestroy), map((x) => this.addOrUpdate(x)))
      .subscribe();
  }

  public addOrUpdate(behaviourType: BehaviourType) {
    if (!behaviourType) return;

    let behaviourTypes = [...this.behaviourTypes$.value];
    const i = behaviourTypes.findIndex((t) => t.behaviourTypeId == behaviourType.behaviourTypeId);
    const _ = i < 0 ? behaviourTypes.push(behaviourType) : behaviourTypes[i] = behaviourType;    
    this.behaviourTypes$.next(behaviourTypes);
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

