import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./edit-behaviour-type-overlay.component.html",
  styleUrls: ["./edit-behaviour-type-overlay.component.css"],
  selector: "app-edit-behaviour-type-overlay"
})
export class EditBehaviourTypeOverlayComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  public behaviourTypeId: number;

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
