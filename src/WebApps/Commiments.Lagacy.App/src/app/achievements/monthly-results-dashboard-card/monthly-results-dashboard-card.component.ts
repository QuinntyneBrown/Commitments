// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";

@Component({
  templateUrl: "./monthly-results-dashboard-card.component.html",
  styleUrls: ["./monthly-results-dashboard-card.component.scss"],
  selector: "app-monthly-results-dashboard-card"
})
export class MonthlyResultsDashboardCardComponent extends DashboardCardComponent { 
  constructor(elementRef: ElementRef) {
    super(elementRef);
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}

