// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
import { ToDoService } from '../../services/to-do.service';
import { ToDo } from '../../models/to-do';
import { EditToDoDialogService } from '../../services/edit-to-do-dialog.service';
import { CheckboxCellComponent } from '../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '../../components/primary-header/primary-header.component';

@Component({
  selector: 'app-to-dos-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './to-dos-page.component.html',
  styleUrls: ['./to-dos-page.component.scss']
})
export class ToDosPageComponent {
  private readonly _editToDoDialog = inject(EditToDoDialogService);
  private readonly _toDoService = inject(ToDoService);

  constructor() {
    this.handleRemoveToDoCellClick = this.handleRemoveToDoCellClick.bind(this);
  }

  public ngOnInit() {
    this._toDoService.get()
      .pipe(map(x => this.toDos$.next(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name" },
    { headerName: "Due On", field: "dueOn" },
    { headerName: "Completed On", field: "completedOn" },
    { cellRenderer: "editRenderer", onCellClicked: $event => this.handleEditToDoCellClick($event), width: 50 },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemoveToDoCellClick($event), width: 50 }
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

  public toDos$: BehaviorSubject<Array<ToDo>> = new BehaviorSubject([]);

  public toDosBehaviourSubject$: BehaviorSubject<Array<ToDo>> = new BehaviorSubject([]);

  public handleFabButtonClick() {
    const overlayRefWrapper = this._editToDoDialog.create()
      .pipe(map(toDo => this.addOrUpdate(toDo)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditToDoCellClick($event) {
    this._editToDoDialog.create({ toDoId: $event.data.toDoId })
      .pipe(map(toDo => this.addOrUpdate(toDo)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleRemoveToDoCellClick($event) {
    const toDo = $event.data;

    const toDos: Array<ToDo> = [...this.toDos$.value];
    const index = toDos.findIndex(x => x.toDoId == $event.data.toDoId);
    toDos.splice(index, 1);
    this.toDos$.next(toDos);

    this._toDoService.remove({ toDo })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public addOrUpdate(toDo: ToDo) {
    if (!toDo) return;

    let toDos = [...this.toDos$.value];
    const i = toDos.findIndex((t) => t.toDoId == toDo.toDoId);

    if (i < 0) {
      toDos.push(toDo);
    } else {
      toDos[i] = toDo;
    }

    this.toDos$.next(toDos);
  }
}
