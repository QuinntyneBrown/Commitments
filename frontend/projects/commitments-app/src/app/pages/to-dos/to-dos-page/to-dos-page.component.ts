// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { tap } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid-community';
import { ToDoService } from '../../../services/to-do.service';
import { ToDo } from '../../../models/to-do';
import { EditToDoDialogService } from '../../../services/edit-to-do-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly toDos = signal<Array<ToDo>>([]);

  public localeText: any = {};

  constructor() {
    this.handleRemoveToDoCellClick = this.handleRemoveToDoCellClick.bind(this);
  }

  public ngOnInit() {
    this._toDoService.get()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.toDos.set(x)))
      .subscribe();
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

  public handleFabButtonClick() {
    this._editToDoDialog.create()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(toDo => this.addOrUpdate(toDo)))
      .subscribe();
  }

  public handleEditToDoCellClick($event) {
    this._editToDoDialog.create({ toDoId: $event.data.toDoId })
      .pipe(takeUntilDestroyed(this._destroyRef), tap(toDo => this.addOrUpdate(toDo)))
      .subscribe();
  }

  public handleRemoveToDoCellClick($event) {
    const toDo = $event.data;

    this.toDos.update(toDos => toDos.filter(x => x.toDoId != toDo.toDoId));

    this._toDoService.remove({ toDo })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public addOrUpdate(toDo: ToDo) {
    if (!toDo) return;

    this.toDos.update(toDos => {
      const next = [...toDos];
      const i = next.findIndex(t => t.toDoId == toDo.toDoId);
      if (i < 0) {
        next.push(toDo);
      } else {
        next[i] = toDo;
      }
      return next;
    });
  }
}
