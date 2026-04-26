// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { BehaviourService } from '../../services/behaviour.service';
import { Behaviour } from '../../models/behaviour';
import { ActivityService } from '../../services/activity.service';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { Activity } from '../../models/activity';
import { takeUntil, map } from 'rxjs';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-edit-activity-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    TranslateModule
  ],
  templateUrl: './edit-activity-dialog.html',
  styleUrls: ['./edit-activity-dialog.scss']
})
export class EditActivityDialogComponent {
  private readonly _activityService = inject(ActivityService);
  private readonly _behaviourService = inject(BehaviourService);
  private readonly _overlay = inject(OverlayRefWrapper);

  public activityId: number;

  public handleSaveClick() {
    let activity = new Activity();

    activity.activityId = this.activityId;
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

    this._activityService.getById({ activityId: this.activityId })
      .pipe(map(x => this.form.patchValue({
        performedOn: x.performedOn,
        behaviourId: x.behaviourId,
        description: x.description
      })))
      .subscribe();
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
