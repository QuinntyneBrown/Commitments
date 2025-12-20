// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { CommitmentService } from "./commitment.service";
import { map, takeUntil } from "rxjs";
import { EditCommitmentDialogService } from "./edit-commitment-dialog";
import { Commitment } from "./commitment";
import { ColDef, GridApi } from "ag-grid";
import { CheckboxCellComponent } from "../shared/checkbox-cell.component";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";

@Component({
  templateUrl: "./my-commiments-page.component.html",
  styleUrls: ["./my-commiments-page.component.scss"],
  selector: "app-my-commiments-page"
})
export class MyCommimentsPageComponent { 
  constructor(
    private readonly _commitmentService: CommitmentService,
    private readonly _editCommitmentDialog: EditCommitmentDialogService
    
  ) { }

  ngOnInit() {
    this._commitmentService.getPersonal()
      .pipe(takeUntil(this.onDestroy),map(x => this.commitments$.next(x)))
      .subscribe();
  }

  public commitments$: BehaviorSubject<Commitment[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Type", field: "behaviour.behaviourType.name" },
    { headerName: "Name", field: "behaviour.name" },
    { cellRenderer: "editRenderer", onCellClicked: $event => this.handleEditClick($event), width:50 },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemoveClick($event), width: 50 }
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

  public handleFABButtonClick() {
    this._editCommitmentDialog.create()
      .pipe(takeUntil(this.onDestroy))
      .subscribe()    
  }


  public handleEditClick($event) {
    this._editCommitmentDialog.create({ commitmentId: $event.data.commitmentId })
      .pipe(map(commitment => this.addOrUpdate(commitment)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleRemoveClick($event) {
    const commitment = $event.data;

    const commitments: Array<Commitment> = [...this.commitments$.value];
    const index = commitments.findIndex(x => x.commitmentId == $event.data.commitmentId);
    commitments.splice(index, 1);
    this.commitments$.next(commitments);

    this._commitmentService.remove({ commitment })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public addOrUpdate(commitment: Commitment) {
    if (!commitment) return;

    let commitments = [...this.commitments$.value];
    const i = commitments.findIndex((t) => t.commitmentId == commitment.commitmentId);

    if (i < 0) {
      commitments.push(commitment);
    } else {
      commitments[i] = commitment;
    }

    this.commitments$.next(commitments);
  }
}

