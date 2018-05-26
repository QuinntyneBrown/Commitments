import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { EditActivityOverlayComponent } from "./edit-activity-overlay.component";
import { map, takeUntil, tap } from "rxjs/operators";
import { Router } from "@angular/router";
import { EditActivityOverlay } from "./edit-activity-overlay";
import { ActivityService } from "./activity.service";

@Component({
  templateUrl: "./activities-page.component.html",
  styleUrls: ["./activities-page.component.css"],
  selector: "app-activities-page"
})
export class ActivitiesPageComponent { 
  constructor(
    private _activityService: ActivityService,
    private _editActivityOverlay: EditActivityOverlay,
    private _router: Router
  ) { }

  public onDestroy: Subject<void> = new Subject<void>();

  public handleFabButtonClick() {
    this._editActivityOverlay.create()
      .pipe(takeUntil(this.onDestroy), tap(() => this._router.navigateByUrl("/")))
      .subscribe();
  }

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}
