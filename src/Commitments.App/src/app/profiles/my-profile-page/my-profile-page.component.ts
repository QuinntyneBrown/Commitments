// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, Inject } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile";
import { takeUntil, map, tap } from "rxjs";
import { LocalStorageService } from "../core/local-storage.service";
import { currentProfileIdKey, baseUrl } from "../core/constants";
import { FormGroup, FormControl } from "@angular/forms";

@Component({
  templateUrl: "./my-profile-page.component.html",
  styleUrls: ["./my-profile-page.component.scss"],
  selector: "app-my-profile-page"
})
export class MyProfilePageComponent { 
  constructor(
    @Inject(baseUrl) public baseUrl: string,
    private readonly _profileService: ProfileService,
    private readonly _storage: LocalStorageService
  ) { }

  public ngOnInit() {
    this.profile$ = this._profileService.current()
      .pipe(
        tap(x => this.form.patchValue({
          name: x.name,
          avatarUrl: x.avatarUrl
        }))
      );
  }

  public handleSaveClick() {
    this._profileService.saveAvatarUrl(
      {
        avatarUrl: this.form.value.avatarUrl,
        profileId: this.profileId
      })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public profile$: Observable<Profile>;
  
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public get profileId(): number {
    return +this._storage.get({ name: currentProfileIdKey });
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null,[]),
    avatarUrl: new FormControl(null,[])
  });
}

