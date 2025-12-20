// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { DashboardCard } from "../dashboard-card";

@Component({
  templateUrl: "./poster-dashboard-card.html",
  styleUrls: ["./poster-dashboard-card.scss"],
  selector: "app-poster-dashboard-card"
})
export class PosterDashboardCard extends DashboardCard {
  constructor(elementRef: ElementRef) {
    super(elementRef);
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public title: string;

  public value$: Observable<number>;
}
