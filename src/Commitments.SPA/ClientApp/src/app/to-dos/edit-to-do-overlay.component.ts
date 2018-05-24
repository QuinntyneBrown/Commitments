import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ToDoService } from "./to-do.service";
import { ToDo } from "./to-do.model";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { map, takeUntil, tap } from "rxjs/operators";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-to-do-overlay.component.html",
  styleUrls: ["./edit-to-do-overlay.component.css"],
  selector: "app-edit-to-do-overlay"
})
export class EditToDoOverlayComponent { 
  constructor(
    private _overlay: OverlayRefWrapper,
    private _toDoService: ToDoService) { }

  public ngOnInit() {
    if (this.toDoId)
      this._toDoService
        .getById({ toDoId: this.toDoId })
        .pipe(map(toDo => this.form.patchValue({
          name: toDo.name,
          description: toDo.description,
          dueOn: toDo.dueOn
        })), takeUntil(this.onDestroy))
        .subscribe();
  }

  public handleSaveClick() {
    const toDo = new ToDo();
    toDo.toDoId = this.toDoId;
    toDo.description = this.form.value.description;
    toDo.dueOn = this.form.value.dueOn;
    toDo.name = this.form.value.name;
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
  
  public get toDoId(): number { return this._overlay.data.toDoId; }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    dueOn: new FormControl(null, [Validators.required])
  });
}
