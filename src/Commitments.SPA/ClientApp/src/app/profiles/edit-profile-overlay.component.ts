import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./edit-profile-overlay.component.html",
  styleUrls: ["./edit-profile-overlay.component.css"],
  selector: "app-edit-profile-overlay"
})
export class EditProfileOverlayComponent { 
  constructor(
    private _profileService: ProfileService,
    private _overlay: OverlayRefWrapper) { }

  ngOnInit() {
    if (this.profileId)
      this._profileService.getById({ profileId: this.profileId })
        .pipe(
          map(x => this.profile$.next(x)),
          switchMap(x => this.profile$),
          map(x => this.form.patchValue({
            name: x.name
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public profile$: BehaviorSubject<Profile> = new BehaviorSubject(<Profile>{});
  
  public profileId: number;

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const profile = new Profile();
    profile.profileId = this.profileId;
    profile.name = this.form.value.name;
    this._profileService.save({ profile })
      .pipe(
        map(x => profile.vId = x.profileId),
        tap(x => this._overlay.close(profile)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
} 
