import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./my-profile-page.component.html",
  styleUrls: ["./my-profile-page.component.css"],
  selector: "app-my-profile-page"
})
export class MyProfilePageComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
