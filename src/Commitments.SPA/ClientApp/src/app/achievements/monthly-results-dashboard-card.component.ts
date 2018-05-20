import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./monthly-results-dashboard-card.component.html",
  styleUrls: ["./monthly-results-dashboard-card.component.css"],
  selector: "app-monthly-results-dashboard-card"
})
export class MonthlyResultsDashboardCardComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
