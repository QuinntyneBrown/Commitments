import { Component, ElementRef } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";

@Component({
  templateUrl: "./weekly-results-dashboard-card.component.html",
  styleUrls: ["./weekly-results-dashboard-card.component.css"],
  selector: "app-weekly-results-dashboard-card"
})
export class WeeklyResultsDashboardCardComponent extends DashboardCardComponent { 

  constructor(elementRef: ElementRef) {
    super(elementRef);
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}
