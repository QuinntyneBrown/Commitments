import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile.model";
import { takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./my-profile-page.component.html",
  styleUrls: ["./my-profile-page.component.css"],
  selector: "app-my-profile-page"
})
export class MyProfilePageComponent { 
  constructor(
    private _profileService: ProfileService
  ) { }

  public ngOnInit() {
    this.profile$ = this._profileService.current();
  }

  public handleSaveAvatarClick(avatarUrl:string) {
    this._profileService.saveAvatarUrl({ avatarUrl })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public profile$: Observable<Profile>;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
