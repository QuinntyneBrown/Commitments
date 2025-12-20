// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ToDoService } from "./to-do.service";
import { ToDo } from "./to-do";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { map, takeUntil, tap } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-to-do-dialog.html",
  styleUrls: ["./edit-to-do-dialog.scss"],
  selector: "app-edit-to-do-dialog"
})
export class EditToDoDialog {
  constructor(
    private readonly _overlay: OverlayRefWrapper,
    private readonly _toDoService: ToDoService) { }

  public ngOnInit() {
    if (this.toDoId)
      this._toDoService
        .getById({ toDoId: this.toDoId })
        .pipe(map(toDo => this.form.patchValue({
          name: toDo.name,
          description: toDo.description,
          dueOn: toDo.dueOn,
          completedOn: toDo.completedOn
        })), takeUntil(this.onDestroy))
        .subscribe();
  }

  public handleSaveClick() {
    const toDo = new ToDo();
    toDo.toDoId = this.toDoId;
    toDo.description = this.form.value.description;
    toDo.dueOn = this.form.value.dueOn;
    toDo.completedOn = this.form.value.completedOn
    toDo.name = this.form.value.name;
    toDo.isCompleted = !this.form.value.completedOn;
    this._toDoService.save({ toDo })
      .pipe(
        map(x => toDo.toDoId = x.toDoId),
        tap(x => this._overlay.close(toDo)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public toDoId: number;

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    dueOn: new FormControl(null, [Validators.required]),
    completedOn: new FormControl(null,[Validators.required])
  });
}

