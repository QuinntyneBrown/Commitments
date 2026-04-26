// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { DashboardCardComponent } from '../dashboard-card/dashboard-card.component';

@Component({
  selector: 'app-weekly-results-dashboard-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './weekly-results-dashboard-card.component.html',
  styleUrls: ['./weekly-results-dashboard-card.component.scss']
})
export class WeeklyResultsDashboardCardComponent extends DashboardCardComponent {
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
