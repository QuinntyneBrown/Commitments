import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./to-do-dashboard-card.component.html",
  styleUrls: ["./to-do-dashboard-card.component.css"],
  selector: "app-to-do-dashboard-card"
})
export class ToDoDashboardCardComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
