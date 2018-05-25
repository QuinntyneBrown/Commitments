import { Component, ElementRef } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";
import { ToDoService } from "./to-do.service";

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
    _toDoService: ToDoService
  ) {
    super(elementRef);
  }

  ngOnInit() {

  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
