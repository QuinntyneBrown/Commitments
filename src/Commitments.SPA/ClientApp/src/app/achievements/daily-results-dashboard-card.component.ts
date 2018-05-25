import { Component, ElementRef } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ActivityService } from "../activities/activity.service";
import { CommitmentService } from "../commitments/commitment.service";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";
import {  } from "@aspnet/signalr";
import { Achievement } from "./achievement.model";
import { Commitment } from "../commitments/commitment.model";
import { map } from "rxjs/operators";
import { AchievementService } from "./achievement.service";

@Component({
  templateUrl: "./daily-results-dashboard-card.component.html",
  styleUrls: [
    "../dashboard-cards/dashboard-card.component.css",
    "./daily-results-dashboard-card.component.css"
  ],
  selector: "app-daily-results-dashboard-card"
})
export class DailyResultsDashboardCardComponent extends DashboardCardComponent { 
  constructor(
    _elementRef: ElementRef,
    private _achievementService: AchievementService,
    private _commitmentService: CommitmentService
  ) {
    super(_elementRef);
  }

  ngOnInit() {
    this.achievements$ = this._achievementService.get().pipe(map(x => x.length));
    this.commitments$ = this._commitmentService.getDailyByCurrentProfile().pipe(map(x => x.length));
  }

  public onDestroy: Subject<void> = new Subject<void>();
  
  public achievements$: Observable<number>;

  public commitments$: Observable<number>;

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}
