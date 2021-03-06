import { Component, ElementRef } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ActivityService } from "../activities/activity.service";
import { CommitmentService } from "../commitments/commitment.service";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";
import { } from "@aspnet/signalr";
import { Achievement } from "./achievement.model";
import { Commitment } from "../commitments/commitment.model";
import { map } from "rxjs/operators";
import { AchievementService } from "./achievement.service";

@Component({
  templateUrl: "./relations-results-dashboard-card.component.html",
  styleUrls: [
    "../dashboard-cards/dashboard-card.component.css",
    "./relations-results-dashboard-card.component.css"
  ],
  selector: "app-daily-results-dashboard-card"
})
export class RelationsResultsDashboardCardComponent extends DashboardCardComponent {
  constructor(
    _elementRef: ElementRef,
  ) {
    super(_elementRef);
  }

  ngOnInit() {

  }

  public onDestroy: Subject<void> = new Subject<void>();

  public achievements$: Observable<number>;

  public commitments$: Observable<number>;

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
