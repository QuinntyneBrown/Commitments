import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./dashboard-page.component.html",
  styleUrls: ["./dashboard-page.component.css"],
  selector: "app-dashboard-page"
})
export class DashboardPageComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
