import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./profiles-page.component.html",
  styleUrls: ["./profiles-page.component.css"],
  selector: "app-profiles-page"
})
export class ProfilesPageComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
