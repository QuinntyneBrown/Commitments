// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile";
import { map, switchMap, tap, takeUntil } from "rxjs";

@Component({
  templateUrl: "./create-profile-dialog.html",
  styleUrls: ["./create-profile-dialog.scss"],
  selector: "app-create-profile-dialog"
})
export class CreateProfileDialog {
  constructor(
    private readonly _profileService: ProfileService,
    private readonly _overlay: OverlayRefWrapper) { }

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

