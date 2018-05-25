import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ProfileService } from "./profile.service";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-profile-overlay.component.html",
  styleUrls: ["./edit-profile-overlay.component.css"],
  selector: "app-edit-profile-overlay"
})
export class EditProfileOverlayComponent { 
  constructor(
    private _profileService: ProfileService,
    private _overlay: OverlayRefWrapper
  ) {

  }
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
