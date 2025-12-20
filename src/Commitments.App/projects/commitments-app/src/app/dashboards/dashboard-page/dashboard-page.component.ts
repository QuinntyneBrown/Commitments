// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Overlay } from "@angular/cdk/overlay";
import { Component, ComponentFactoryResolver, ComponentRef, Injector, ViewChild, ViewContainerRef } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";
import { filter, map, switchMap, takeUntil, tap } from "rxjs";
import { DailyResultsDashboardCardComponent } from "../achievements/daily-results-dashboard-card.component";
import { MonthlyResultsDashboardCardComponent } from "../achievements/monthly-results-dashboard-card.component";
import { WeeklyResultsDashboardCardComponent } from "../achievements/weekly-results-dashboard-card.component";
import { deepCopy } from "../core/deep-copy";
import { AddDashboardCardsDialogService } from "../dashboard-cards/add-dashboard-cards-dialog";
import { DashboardCardConfigurationDialogService } from "../dashboard-cards/dashboard-card-configuration-dialog";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";
import { DashboardCard } from "../dashboard-cards/dashboard-card";
import { DashboardCardService } from "../dashboard-cards/dashboard-card.service";
import { ToDoDashboardCardComponent } from "../to-dos/to-do-dashboard-card.component";
import { Dashboard } from "./dashboard";
import { DashboardService } from "./dashboard.service";
import { RelationsResultsDashboardCardComponent } from "../achievements/relations-results-dashboard-card.component";

@Component({
  templateUrl: "./dashboard-page.component.html",
  styleUrls: ["./dashboard-page.component.scss"],
  selector: "app-dashboard-page"
})
export class DashboardPageComponent {
  constructor(
    private readonly _addDashboardCardsDialogService: AddDashboardCardsDialogService,
    private readonly _componentFactoryResolver: ComponentFactoryResolver,
    private readonly _dashboardCardConfigurationDialogService: DashboardCardConfigurationDialogService,
    private readonly _dashboardCardService: DashboardCardService,
    private readonly _dashboardService: DashboardService,    
    private readonly _injector: Injector,
    private readonly _overlay: Overlay
  ) { }

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
      .subscribe()   
  }

  public addDashboardCardComponentRef(dashboardCard: DashboardCard) {
    let componentFactory: ComponentRef<DashboardCardComponent>;

    switch (dashboardCard.cardId) {
      case 1:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(DailyResultsDashboardCardComponent))
        break;
      case 2:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(WeeklyResultsDashboardCardComponent))
        break;
      case 3:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(MonthlyResultsDashboardCardComponent))
        break;
      case 4:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(ToDoDashboardCardComponent))
        break;
      case 6:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(RelationsResultsDashboardCardComponent))
        break;
      default:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(DashboardCardComponent));
        break;
    }

    const dashboardCardComponentRef: ComponentRef<DashboardCardComponent> = this.target.createComponent(<any>componentFactory, null, this._injector);

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

