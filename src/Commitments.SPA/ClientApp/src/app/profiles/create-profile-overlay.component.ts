import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./create-profile-overlay.component.html",
  styleUrls: ["./create-profile-overlay.component.css"],
  selector: "app-create-profile-overlay"
})
export class CreateProfileOverlayComponent { 
  constructor(
    private _profileService: ProfileService,
    private _overlay: OverlayRefWrapper) { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
  
  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const options = {
      username: this.form.value.username,
      name: this.form.value.name,
      avatarUrl: this.form.value.avatarUrl,
      password: this.form.value.password,
      confirmPassword: this.form.value.confirmPassword
    };

    const profile = new Profile();

    profile.name = options.name;
    this._profileService.create(options)
      .pipe(
        map(x => profile.profileId = x.profileId),
        tap(x => this._overlay.close(profile)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    avatarUrl: new FormControl(null,[]),
    username: new FormControl(null, []),
    name: new FormControl(null,[]),
    password: new FormControl(null, []),
    confirmPassword: new FormControl(null, [])
  });
} 
