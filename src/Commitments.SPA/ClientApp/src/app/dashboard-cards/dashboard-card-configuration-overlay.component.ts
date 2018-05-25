import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { DashboardCard } from "./dashboard-card.model";
import { DashboardCardService } from "./dashboard-card.service";

@Component({
  templateUrl: "./dashboard-card-configuration-overlay.component.html",
  styleUrls: ["./dashboard-card-configuration-overlay.component.css"],
  selector: "app-dashboard-card-configuration-overlay"
})
export class DashboardCardConfigurationOverlayComponent { 
  constructor(
    private _dashboardCardService: DashboardCardService,
    private _overlay: OverlayRefWrapper) { }

  public close() {
    this._overlay.close();
  }

  public handleSaveClick() {

  }

  public dashboardCard: DashboardCard;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
