import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ToDoService } from "./to-do.service";
import { ToDo } from "./to-do.model";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { map, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./edit-to-do-overlay.component.html",
  styleUrls: ["./edit-to-do-overlay.component.css"],
  selector: "app-edit-to-do-overlay"
})
export class EditToDoOverlayComponent { 
  constructor(private _toDoService: ToDoService) { }

  public ngOnInit() {
    if (this.toDoId)
      this._toDoService
        .getById({ toDoId: this.toDoId })
        .pipe(map(toDo => this.formGroup.patchValue({
          name: toDo.name,
          description: toDo.description,
          dueOn: toDo.dueOn
        })), takeUntil(this.onDestroy))
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
  
  public toDoId: number;

  public formGroup: FormGroup = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    dueOn: new FormControl(null, [Validators.required])
  });
}
