import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./dashboard-card.component.html",
  styleUrls: ["./dashboard-card.component.css"],
  selector: "app-dashboard-card"
})
export class DashboardCardComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
