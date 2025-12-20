// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { takeUntil, tap, map } from "rxjs";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { CardLayoutService } from "./card-layout.service";
import { CardLayout } from "./card-layout";
import { deepCopy } from "../core/deep-copy";
import { GridApi, ColDef } from "ag-grid";
import { CheckboxCellComponent } from "../shared/checkbox-cell.component";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";
import { EditCardLayoutDialogService } from "./edit-card-layout-dialog";

@Component({
  templateUrl: "./card-layouts-page.component.html",
  styleUrls: ["./card-layouts-page.component.scss"],
  selector: "app-card-layouts-page"
})
export class CardLayoutsPageComponent { 
  constructor(
    private readonly _cardLayoutService: CardLayoutService,
    private readonly _editCardLayoutDialog: EditCardLayoutDialogService,
    private readonly _router: Router
  ) { }

  ngOnInit() {
    this._cardLayoutService.get()
      .pipe(map(x => this.cardLayouts$.next(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public cardLayouts$: BehaviorSubject<Array<CardLayout>> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  public handleRemoveClick($event) {
    const cardLayouts: Array<CardLayout> = [...this.cardLayouts$.value];
    const index = cardLayouts.findIndex(x => x.cardLayoutId == $event.data.cardLayoutId);
    cardLayouts.splice(index, 1);
    this.cardLayouts$.next(cardLayouts);

    this._cardLayoutService.remove({ cardLayout: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {    

  }

  public addOrUpdate(cardLayout: CardLayout) {
    if (!cardLayout) return;

    let cardLayouts = [...this.cardLayouts$.value];
    const i = cardLayouts.findIndex((t) => t.cardLayoutId == cardLayout.cardLayoutId);
    const _ = i < 0 ? cardLayouts.push(cardLayout) : cardLayouts[i] = cardLayout;    
    this.cardLayouts$.next(cardLayouts);
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

  public handleFABButtonClick() {
    this._editCardLayoutDialog.create()
      .pipe(takeUntil(this.onDestroy), map(x => this.addOrUpdate(x)))
      .subscribe();

  }
}

