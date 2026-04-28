// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ToDoService } from '../../services/to-do.service';
import { ToDo } from '../../models/to-do';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { tap } from 'rxjs';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';

@Component({
  selector: 'app-edit-to-do-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-to-do-dialog.html',
  styleUrls: ['./edit-to-do-dialog.scss']
})
export class EditToDoDialogComponent {
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _toDoService = inject(ToDoService);
  private readonly _destroyRef = inject(DestroyRef);

  public ngOnInit() {
    if (this.toDoId) {
      this._toDoService
        .getById({ toDoId: this.toDoId })
        .pipe(
          takeUntilDestroyed(this._destroyRef),
          tap(toDo => this.form.patchValue({
            name: toDo.name,
            description: toDo.description,
            dueOn: toDo.dueOn,
            completedOn: toDo.completedOn
          }))
        )
        .subscribe();
    }
  }

  public handleSaveClick() {
    const toDo = new ToDo();
    toDo.toDoId = this.toDoId;
    toDo.description = this.form.value.description;
    toDo.dueOn = this.form.value.dueOn;
    toDo.completedOn = this.form.value.completedOn;
    toDo.name = this.form.value.name;
    toDo.isCompleted = !this.form.value.completedOn;
    this._toDoService.save({ toDo })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          toDo.toDoId = x.toDoId;
          this._overlay.close(toDo);
        })
      )
      .subscribe();
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public toDoId: number;

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    dueOn: new FormControl(null, [Validators.required]),
    completedOn: new FormControl(null, [Validators.required])
  });
}
