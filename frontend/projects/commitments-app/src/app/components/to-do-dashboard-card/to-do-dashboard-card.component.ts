// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { ToDoService } from '../../services/to-do.service';
import { takeUntil, map } from 'rxjs';
import { PosterDashboardCardComponent } from '../poster-dashboard-card/poster-dashboard-card.component';

@Component({
  selector: 'app-to-do-dashboard-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: '../poster-dashboard-card/poster-dashboard-card.component.html',
  styleUrls: [
    '../dashboard-card/dashboard-card.component.scss',
    '../poster-dashboard-card/poster-dashboard-card.component.scss',
    './to-do-dashboard-card.component.scss'
  ]
})
export class ToDoDashboardCardComponent extends PosterDashboardCardComponent {
  private readonly _toDoService = inject(ToDoService);

  ngOnInit() {
    this.title = 'Outstanding To Dos';
    this.value$ = this._toDoService.getOutstandingToDos()
      .pipe(
        map(x => x.length), takeUntil(this.onDestroy)
      );
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
