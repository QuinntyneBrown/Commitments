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
import { BehaviourTypeService } from '../../services/behaviour-type.service';
import { BehaviourType } from '../../models/behaviour-type';
import { EditBehaviourTypeDialogService } from '../../services/edit-behaviour-type-dialog.service';
import { CheckboxCellComponent } from '../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '../../components/primary-header/primary-header.component';

@Component({
  selector: 'app-behaviour-types-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './behaviour-types-page.component.html',
  styleUrls: ['./behaviour-types-page.component.scss']
})
export class BehaviourTypesPageComponent {
  private readonly _behaviourTypeService = inject(BehaviourTypeService);
  private readonly _editBehaviourTypeDialog = inject(EditBehaviourTypeDialogService);
  private readonly _router = inject(Router);

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

  private _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }
}
