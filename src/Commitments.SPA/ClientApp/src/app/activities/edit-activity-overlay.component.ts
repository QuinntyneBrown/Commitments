import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { BehaviourService } from "../behaviours/behaviour.service";
import { Behaviour } from "../behaviours/behaviour.model";
import { ActivityService } from "./activity.service";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { Activity } from "./activity.model";
import { takeUntil, map } from "rxjs/operators";
import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
  templateUrl: "./edit-activity-overlay.component.html",
  styleUrls: ["./edit-activity-overlay.component.css"],
  selector: "app-edit-activity-overlay"
})
export class EditActivityOverlayComponent {
  constructor(
    private _activityService: ActivityService,
    private _behaviourService: BehaviourService,
    private _overlay: OverlayRefWrapper
  ) { }

  public activityId: number;

  public handleSaveClick() {
    let activity = new Activity();

    activity.behaviourId = this.form.value.behaviourId;
    activity.performedOn = this.form.value.performedOn;
    activity.description = this.form.value.description;

    this._activityService.save({ activity })
      .pipe(map((x: { activityId: number }) => {
        activity.activityId = x.activityId;
        this._overlay.close(activity);
      }), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleCancelClick() {
    this._overlay.close();
  }
  ngOnInit() {
    this.behaviours$ = this._behaviourService.get();
  }

  public behaviours$: Observable<Array<Behaviour>>;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public form: FormGroup = new FormGroup({
    performedOn: new FormControl(null, [Validators.required]),
    behaviourId: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [])
  });
}
