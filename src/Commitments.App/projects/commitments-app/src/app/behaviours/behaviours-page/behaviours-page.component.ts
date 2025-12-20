// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { takeUntil, tap, map } from "rxjs";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { BehaviourService } from "./behaviour.service";
import { Behaviour } from "./behaviour";
import { BehaviourType } from "./behaviour-type";
import { EditBehaviourDialogService } from "./edit-behaviour-dialog";
import { deepCopy } from "../core/deep-copy";
import { GridApi, ColDef } from "ag-grid";
import { CheckboxCellComponent } from "../shared/checkbox-cell.component";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";
import { BehaviourTypeService } from "../behaviour-types/behaviour-type.service";

@Component({
  templateUrl: "./behaviours-page.component.html",
  styleUrls: ["./behaviours-page.component.scss"],
  selector: "app-edit-behaviour-page"
})
export class BehavioursPageComponent { 
  constructor(
    private readonly _behaviourService: BehaviourService,
    private readonly _behaviourTypeService: BehaviourTypeService,
    private readonly _editBehaviourDialog: EditBehaviourDialogService,
    private readonly _router: Router
  ) { }

  ngOnInit() {
    this._behaviourService.get()
      .pipe(map(x => this.behaviours$.next(x)))
      .subscribe();
  }

  public behaviourTypes$: Observable<Array<BehaviourType>>;

  public onDestroy: Subject<void> = new Subject<void>();

  public behaviour$: BehaviorSubject<Behaviour> = new BehaviorSubject(<Behaviour>{});
  
  public behaviours$: BehaviorSubject<Array<Behaviour>> = new BehaviorSubject([]);

  public handleFABButtonClick() {
    this._editBehaviourDialog.create({ behaviourId: this.behaviour$.value.behaviourId })
      .pipe(map(toDo => this.addOrUpdate(toDo)), takeUntil(this.onDestroy))
      .subscribe();
  }

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  public handleRemoveClick($event) {
    const behaviours: Array<Behaviour> = [...this.behaviours$.value];
    const index = behaviours.findIndex(x => x.behaviourId == $event.data.behaviourId);
    behaviours.splice(index, 1);
    this.behaviours$.next(behaviours);

    this._behaviourService.remove({ behaviour: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {    
    this._editBehaviourDialog.create({ behaviourId: $event.data.behaviourId })
      .pipe(map(toDo => this.addOrUpdate(toDo)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public addOrUpdate(behaviour: Behaviour) {
    if (!behaviour) return;

    let behaviours = [...this.behaviours$.value];
    const i = behaviours.findIndex((t) => t.behaviourId == behaviour.behaviourId);
    const _ = i < 0 ? behaviours.push(behaviour) : behaviours[i] = behaviour;    
    this.behaviours$.next(behaviours);
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

