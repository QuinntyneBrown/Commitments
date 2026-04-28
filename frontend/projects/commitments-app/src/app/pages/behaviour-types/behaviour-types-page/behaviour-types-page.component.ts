// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid-community';
import { BehaviourTypeService } from '../../../services/behaviour-type.service';
import { BehaviourType } from '../../../models/behaviour-type';
import { EditBehaviourTypeDialogService } from '../../../services/edit-behaviour-type-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly behaviourTypes = signal<Array<BehaviourType>>([]);

  public localeText: any = {};

  ngOnInit() {
    this._behaviourTypeService.get()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.behaviourTypes.set(x)))
      .subscribe();
  }

  public handleRemoveClick($event) {
    this.behaviourTypes.update(types => types.filter(x => x.behaviourTypeId != $event.data.behaviourTypeId));

    this._behaviourTypeService.remove({ behaviourType: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event) {

  }

  public handleFABButtonClick() {
    this._editBehaviourTypeDialog.create()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.addOrUpdate(x)))
      .subscribe();
  }

  public addOrUpdate(behaviourType: BehaviourType) {
    if (!behaviourType) return;

    this.behaviourTypes.update(types => {
      const next = [...types];
      const i = next.findIndex(t => t.behaviourTypeId == behaviourType.behaviourTypeId);
      if (i < 0) {
        next.push(behaviourType);
      } else {
        next[i] = behaviourType;
      }
      return next;
    });
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
