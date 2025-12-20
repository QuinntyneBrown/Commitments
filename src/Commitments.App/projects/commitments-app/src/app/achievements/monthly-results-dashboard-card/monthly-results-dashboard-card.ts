// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";

@Component({
  templateUrl: "./monthly-results-dashboard-card.html",
  styleUrls: ["./monthly-results-dashboard-card.scss"],
  selector: "app-monthly-results-dashboard-card"
})
export class MonthlyResultsDashboardCard extends DashboardCardComponent {
  constructor(elementRef: ElementRef) {
    super(elementRef);
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
