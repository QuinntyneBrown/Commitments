import { Component, ComponentFactoryResolver, ViewContainerRef, ViewChild, ComponentRef, Injector } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { Dashboard } from "./dashboard.model";
import { DashboardService } from "./dashboard.service";
import { takeUntil, map } from "rxjs/operators";
import { DashboardCard } from "../dashboard-cards/dashboard-card.model";
import { DashboardCardComponent } from "../dashboard-cards/dashboard-card.component";
import { DailyResultsDashboardCardComponent } from "../achievements/daily-results-dashboard-card.component";

@Component({
  templateUrl: "./dashboard-page.component.html",
  styleUrls: ["./dashboard-page.component.css"],
  selector: "app-dashboard-page"
})
export class DashboardPageComponent { 
  constructor(
    private _componentFactoryResolver: ComponentFactoryResolver,
    private _dashboardService: DashboardService,
    private _injector: Injector
  ) {

  }

  @ViewChild("target", { read: ViewContainerRef })
  target: ViewContainerRef;

  ngOnInit() {
    this._dashboardService
      .getByCurrentProfile()
      .pipe(takeUntil(this.onDestroy), map(dashboard => {
        this._dashboardCardComponentRefs.forEach((dtc) => {
          dtc.destroy()
        });

        this._dashboardCardComponentRefs = [];

        dashboard.dashboardCards.forEach((dashboardCard) => this.addDashboardCardComponentRef(dashboardCard));

        return dashboard;
      }))
      .subscribe();
  }

  public handleFabButtonClick() {

  }

  public addDashboardCardComponentRef(dashboardCard: DashboardCard) {
    let componentFactory: ComponentRef<DashboardCardComponent>;

    switch (dashboardCard.cardId) {
      case 1:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(DailyResultsDashboardCardComponent))
        break;
      default:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(DashboardCardComponent));
        break;
    }

    const dashboardCardComponentRef: ComponentRef<DashboardCardComponent> = this.target.createComponent(<any>componentFactory, null, this._injector);

    dashboardCardComponentRef.instance.dashboardCard = dashboardCard;

    this._dashboardCardComponentRefs.push(dashboardCardComponentRef);
  }

  public _dashboardCardComponentRefs: Array<ComponentRef<any>> = [];

  public dashboard$: Observable<Dashboard>;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
