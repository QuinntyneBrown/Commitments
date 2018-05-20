import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./weekly-results-dashboard-card.component.html",
  styleUrls: ["./weekly-results-dashboard-card.component.css"],
  selector: "app-weekly-results-dashboard-card"
})
export class WeeklyResultsDashboardCardComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
