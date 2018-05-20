import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ActivityService } from "../activities/activity.service";
import { CommitmentService } from "../commitments/commitment.service";

@Component({
  templateUrl: "./daily-results-dashboard-card.component.html",
  styleUrls: ["./daily-results-dashboard-card.component.css"],
  selector: "app-daily-results-dashboard-card"
})
export class DailyResultsDashboardCardComponent { 
  constructor(
    private _activityService: ActivityService,
    private _commitmentService: CommitmentService
  ) {

  }
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
