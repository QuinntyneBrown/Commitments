// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { DashboardCardComponent } from '../dashboard-card/dashboard-card.component';

@Component({
  selector: 'app-daily-results-dashboard-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './relations-results-dashboard-card.component.html',
  styleUrls: [
    '../dashboard-card/dashboard-card.component.scss',
    './relations-results-dashboard-card.component.scss'
  ]
})
export class RelationsResultsDashboardCardComponent extends DashboardCardComponent {
  ngOnInit() {}

  public onDestroy: Subject<void> = new Subject<void>();

  public achievements$: Observable<number>;

  public commitments$: Observable<number>;

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
