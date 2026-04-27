// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
import { BehaviourService } from '../../../services/behaviour.service';
import { Behaviour } from '../../../models/behaviour';
import { BehaviourType } from '../../../models/behaviour-type';
import { BehaviourTypeService } from '../../../services/behaviour-type.service';
import { EditBehaviourDialogService } from '../../../services/edit-behaviour-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-edit-behaviour-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './behaviours-page.component.html',
  styleUrls: ['./behaviours-page.component.scss']
})
export class BehavioursPageComponent {
  private readonly _behaviourService = inject(BehaviourService);
  private readonly _behaviourTypeService = inject(BehaviourTypeService);
  private readonly _editBehaviourDialog = inject(EditBehaviourDialogService);
  private readonly _router = inject(Router);

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

    const behaviours = [...this.behaviours$.value];
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
