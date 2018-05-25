import { Component, ElementRef } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";
import { ToDoService } from "./to-do.service";
import { ToDo } from "./to-do.model";
import { takeUntil, map } from "rxjs/operators";

@Component({
  templateUrl: "./to-do-dashboard-card.component.html",
  styleUrls: [
    "../dashboard-cards/dashboard-card.component.css",
    "./to-do-dashboard-card.component.css"
  ],
  selector: "app-to-do-dashboard-card"
})
export class ToDoDashboardCardComponent extends DashboardCardComponent { 
  constructor(
    elementRef: ElementRef,
    private _toDoService: ToDoService
  ) {
    super(elementRef);
  }

  ngOnInit() {
    this._toDoService.getOutstandingToDos()
      .pipe(
        map(x => this.outstandingToDos$.next(x)),takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public outstandingToDos$: BehaviorSubject<ToDo[]> = new BehaviorSubject([]);
}
