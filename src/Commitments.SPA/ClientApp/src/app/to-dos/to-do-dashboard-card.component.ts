import { Component, ElementRef } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { ToDoService } from "./to-do.service";
import { ToDo } from "./to-do.model";
import { takeUntil, map } from "rxjs/operators";
import { PosterDashboardCardComponent } from "../dashboard-cards/poster-dashboard-card.component";

@Component({
  templateUrl: "../dashboard-cards/poster-dashboard-card.component.html",
  styleUrls: [
    "../dashboard-cards/dashboard-card.component.css",
    "../dashboard-cards/poster-dashboard-card.component.css",
    "./to-do-dashboard-card.component.css"
  ],
  selector: "app-to-do-dashboard-card"
})
export class ToDoDashboardCardComponent extends PosterDashboardCardComponent { 
  constructor(
    elementRef: ElementRef,
    private _toDoService: ToDoService
  ) {
    super(elementRef);
  }

  ngOnInit() {
    this.title = "Outstanding To Dos";
    this.value$ = this._toDoService.getOutstandingToDos()
        .pipe(
          map(x => x.length), takeUntil(this.onDestroy)
        );
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }  
}
