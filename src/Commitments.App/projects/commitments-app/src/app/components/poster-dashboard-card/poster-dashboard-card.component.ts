// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { DashboardCardComponent } from '../dashboard-card/dashboard-card.component';

@Component({
  selector: 'app-poster-dashboard-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './poster-dashboard-card.component.html',
  styleUrls: ['./poster-dashboard-card.component.scss']
})
export class PosterDashboardCardComponent extends DashboardCardComponent {
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public title: string;

  public value$: Observable<number>;
}
