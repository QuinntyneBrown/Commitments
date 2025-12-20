// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../../core/overlay-ref-wrapper";
import { DashboardCard } from "../dashboard-card";
import { DashboardCardService } from "../dashboard-card.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { takeUntil, map } from "rxjs";

@Component({
  templateUrl: "./dashboard-card-configuration-dialog.html",
  styleUrls: ["./dashboard-card-configuration-dialog.scss"],
  selector: "app-dashboard-card-configuration-dialog"
})
export class DashboardCardConfigurationDialog {
  constructor(
    private readonly _dashboardCardService: DashboardCardService,
    private readonly _overlay: OverlayRefWrapper) { }

  ngOnInit() {
    this.form.patchValue({
      top: this.dashboardCard.options.top,
      left: this.dashboardCard.options.left,
      height: this.dashboardCard.options.height,
      width: this.dashboardCard.options.width
    });
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    this.dashboardCard.options.top = +this.form.value.top;
    this.dashboardCard.options.left = +this.form.value.left;
    this.dashboardCard.options.height = +this.form.value.height;
    this.dashboardCard.options.width = +this.form.value.width;

    this._dashboardCardService.save({ dashboardCard: this.dashboardCard })
      .pipe(takeUntil(this.onDestroy),map(x => this._overlay.close(this.dashboardCard)))
      .subscribe();
  }

  public dashboardCard: DashboardCard;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public form: FormGroup = new FormGroup({
    top: new FormControl(null, []),
    left: new FormControl(null, []),
    height: new FormControl(null, []),
    width: new FormControl(null, [])
  });
}

