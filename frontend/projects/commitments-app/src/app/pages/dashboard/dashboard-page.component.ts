// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ComponentRef, inject, Injector, ViewChild, ViewContainerRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Overlay } from '@angular/cdk/overlay';
import { BehaviorSubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap } from 'rxjs';
import { deepCopy } from '../../core/deep-copy';
import { AddDashboardCardsDialogService } from '../../services/add-dashboard-cards-dialog.service';
import { DashboardCardConfigurationDialogService } from '../../services/dashboard-card-configuration-dialog.service';
import { DashboardCardComponent } from '../../components/dashboard-card/dashboard-card.component';
import { DashboardCard } from '../../models/dashboard-card';
import { DashboardCardService } from '../../services/dashboard-card.service';
import { DailyResultsDashboardCardComponent } from '../../components/daily-results-dashboard-card/daily-results-dashboard-card.component';
import { MonthlyResultsDashboardCardComponent } from '../../components/monthly-results-dashboard-card/monthly-results-dashboard-card.component';
import { WeeklyResultsDashboardCardComponent } from '../../components/weekly-results-dashboard-card/weekly-results-dashboard-card.component';
import { ToDoDashboardCardComponent } from '../../components/to-do-dashboard-card/to-do-dashboard-card.component';
import { RelationsResultsDashboardCardComponent } from '../../components/relations-results-dashboard-card/relations-results-dashboard-card.component';
import { Dashboard } from '../../models/dashboard';
import { DashboardService } from '../../services/dashboard.service';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss']
})
export class DashboardPageComponent {
  private readonly _addDashboardCardsDialogService = inject(AddDashboardCardsDialogService);
  private readonly _dashboardCardConfigurationDialogService = inject(DashboardCardConfigurationDialogService);
  private readonly _dashboardCardService = inject(DashboardCardService);
  private readonly _dashboardService = inject(DashboardService);
  private readonly _injector = inject(Injector);
  private readonly _overlay = inject(Overlay);

  @ViewChild("target", { read: ViewContainerRef })
  target: ViewContainerRef;

  public handleConfigurationClick(options: { dashboardCard: DashboardCard }) {
    this._dashboardCardConfigurationDialogService.create(options)
      .pipe(
        map(dashboardCard => {
          const currentDashboard: Dashboard = deepCopy(this.dashboard$.value);
          const index = currentDashboard.dashboardCards.findIndex(x => x.dashboardCardId == dashboardCard.dashboardCardId);
          currentDashboard.dashboardCards[index] = dashboardCard;
          this.dashboard$.next(currentDashboard);
        }),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public handleDeleteClick(options: { dashboardCard: DashboardCard }) {
    const currentDashboard: Dashboard = deepCopy(this.dashboard$.value);
    const index = currentDashboard.dashboardCards.findIndex(x => x.dashboardCardId == options.dashboardCard.dashboardCardId);
    currentDashboard.dashboardCards.splice(index, 1);
    this.dashboard$.next(currentDashboard);

    this._dashboardCardService.remove({ dashboardCard: options.dashboardCard })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  ngOnInit() {
    this._dashboardService
      .getByCurrentProfile()
      .pipe(
        takeUntil(this.onDestroy),
        map(x => this.dashboard$.next(x)),
        switchMap(() => this.dashboard$),
        map((dashboard) => {
          this._dashboardCardComponentRefs.forEach((dtc) => dtc.destroy());
          this._dashboardCardComponentRefs = [];
          dashboard.dashboardCards.forEach((dashboardCard) => this.addDashboardCardComponentRef(dashboardCard));
        })
      )
      .subscribe();
  }

  public handleFabButtonClick() {
    this._addDashboardCardsDialogService.create({ dashboardId: this.dashboard$.value.dashboardId })
      .pipe(
        filter(x => x != null),
        map((dashboardCards: Array<DashboardCard>) => {
          const current: Dashboard = deepCopy(this.dashboard$.value);
          dashboardCards.map(dashboardCard => current.dashboardCards.push(dashboardCard));
          this.dashboard$.next(current);
        }),
        takeUntil(this.onDestroy))
      .subscribe();
  }

  public addDashboardCardComponentRef(dashboardCard: DashboardCard) {
    let componentType: any;

    switch (dashboardCard.cardId) {
      case 1:
        componentType = DailyResultsDashboardCardComponent;
        break;
      case 2:
        componentType = WeeklyResultsDashboardCardComponent;
        break;
      case 3:
        componentType = MonthlyResultsDashboardCardComponent;
        break;
      case 4:
        componentType = ToDoDashboardCardComponent;
        break;
      case 6:
        componentType = RelationsResultsDashboardCardComponent;
        break;
      default:
        componentType = DashboardCardComponent;
        break;
    }

    const dashboardCardComponentRef: ComponentRef<DashboardCardComponent> = this.target.createComponent(componentType, { injector: this._injector });

    dashboardCardComponentRef.instance.dashboardCard = dashboardCard;

    dashboardCardComponentRef.instance.onDelete
      .pipe(
        takeUntil(dashboardCardComponentRef.instance.onDestroy),
        tap((dashboardCard) => this.handleDeleteClick({ dashboardCard }))
      ).subscribe();

    dashboardCardComponentRef.instance.onConfigure
      .pipe(
        takeUntil(dashboardCardComponentRef.instance.onDestroy),
        tap((dashboardCard) => this.handleConfigurationClick({ dashboardCard }))
      ).subscribe();

    this._dashboardCardComponentRefs.push(dashboardCardComponentRef);
  }

  public _dashboardCardComponentRefs: Array<ComponentRef<any>> = [];

  public dashboard$: BehaviorSubject<Dashboard> = new BehaviorSubject(null);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
