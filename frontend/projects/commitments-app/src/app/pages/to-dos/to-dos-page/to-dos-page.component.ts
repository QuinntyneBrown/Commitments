// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { ToDoService } from '../../../services/to-do.service';
import { ToDo } from '../../../models/to-do';
import { EditToDoDialogService } from '../../../services/edit-to-do-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type ToDoTableEvent = { data: ToDo };

@Component({
  selector: 'app-to-dos-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './to-dos-page.component.html',
  styleUrls: ['./to-dos-page.component.scss'],
})
export class ToDosPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: ToDo }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: ToDo }>;

  private readonly _editToDoDialog = inject(EditToDoDialogService);
  private readonly _toDoService = inject(ToDoService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly toDos = signal<Array<ToDo>>([]);
  public columns: DataTableColumn<ToDo>[] = [];

  public ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (toDo) => toDo.name },
      { key: 'dueOn', header: 'Due On', cell: (toDo) => toDo.dueOn ?? '' },
      { key: 'completedOn', header: 'Completed On', cell: (toDo) => toDo.completedOn ?? '' },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._toDoService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.toDos.set(x)),
      )
      .subscribe();
  }

  public handleFabButtonClick(): void {
    this._editToDoDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((toDo) => this.addOrUpdate(toDo)),
      )
      .subscribe();
  }

  public handleEditToDoCellClick($event: ToDoTableEvent): void {
    this._editToDoDialog
      .create({ toDoId: $event.data.toDoId })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((toDo) => this.addOrUpdate(toDo)),
      )
      .subscribe();
  }

  public handleRemoveToDoCellClick($event: ToDoTableEvent): void {
    const toDo = $event.data;

    this.toDos.update((toDos) => toDos.filter((x) => x.toDoId != toDo.toDoId));

    this._toDoService.remove({ toDo }).pipe(takeUntilDestroyed(this._destroyRef)).subscribe();
  }

  public addOrUpdate(toDo: ToDo): void {
    if (!toDo) return;

    this.toDos.update((toDos) => {
      const next = [...toDos];
      const i = next.findIndex((t) => t.toDoId == toDo.toDoId);
      if (i < 0) {
        next.push(toDo);
      } else {
        next[i] = toDo;
      }
      return next;
    });
  }
}
