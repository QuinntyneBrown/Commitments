// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef, Input, Output, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardCard } from "./dashboard-card";

@Component({
  templateUrl: "./dashboard-card.component.html",
  styleUrls: ["./dashboard-card.component.scss"],
  selector: "app-dashboard-card"
})
export class DashboardCardComponent { 
  constructor(protected readonly _elementRef: ElementRef) { }

  protected _setCustomProperty(key: string, value: any) {
    this._elementRef.nativeElement.style.setProperty(key, value)
  }


  private _dashboardCard: DashboardCard;

  public onDashboardCardChanged() {

  }

  @Output()
  onConfigure: EventEmitter<any> = new EventEmitter();

  @Output()
  onDelete: EventEmitter<any> = new EventEmitter();

  public configurationMode: boolean;

  @Input()
  public set dashboardCard(value) {
    this._dashboardCard = value;
    this._setCustomProperty('--grid-column-start', this.configurationMode ? 1 : this.dashboardCard.options.left);
    this._setCustomProperty('--grid-row-start', this.configurationMode ? 1 : this.dashboardCard.options.top);
    this._setCustomProperty('--grid-column-stop', (this.configurationMode ? 1 : this.dashboardCard.options.left) + this.dashboardCard.options.width);
    this._setCustomProperty('--grid-row-stop', (this.configurationMode ? 1 : this.dashboardCard.options.top) + this.dashboardCard.options.height);
    this.onDashboardCardChanged();
  }

  public get dashboardCard(): DashboardCard {
    return this._dashboardCard;
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}

