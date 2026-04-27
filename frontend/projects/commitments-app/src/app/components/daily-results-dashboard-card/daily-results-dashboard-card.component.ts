// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { CommitmentService } from '../../services/commitment.service';
import { DashboardCardComponent } from '../dashboard-card/dashboard-card.component';
import { map } from 'rxjs';
import { AchievementService } from '../../services/achievement.service';

@Component({
  selector: 'app-daily-results-dashboard-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './daily-results-dashboard-card.component.html',
  styleUrls: ['./daily-results-dashboard-card.component.scss']
})
export class DailyResultsDashboardCardComponent extends DashboardCardComponent {
  private readonly _achievementService = inject(AchievementService);
  private readonly _commitmentService = inject(CommitmentService);

  ngOnInit() {
    this.achievements$ = this._achievementService.get().pipe(map(x => x.length));
    this.commitments$ = this._commitmentService.getDailyByCurrentProfile().pipe(map(x => x.length));
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public achievements$: Observable<number>;

  public commitments$: Observable<number>;

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
