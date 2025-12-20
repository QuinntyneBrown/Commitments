// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { CreateProfileDialogService } from "./create-profile-dialog";
import { ProfileService } from "./profile.service";
import { Profile } from "./profile";
import { takeUntil, map } from "rxjs";
import { GridApi, ColDef } from "ag-grid";
import { DeleteCellComponent } from "../shared/delete-cell.component";

@Component({
  templateUrl: "./profiles-page.component.html",
  styleUrls: ["./profiles-page.component.scss"],
  selector: "app-profiles-page"
})
export class ProfilesPageComponent { 
  constructor(
    private readonly _createProfileDialog: CreateProfileDialogService,
    private readonly _profileService: ProfileService
  ) {

  }

  public profiles$: BehaviorSubject<Array<Profile>> = new BehaviorSubject([]);

  public ngOnInit() {
    this._profileService.get()
      .pipe(
        map(x => this.profiles$.next(x)), takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public handleFABButtonClick() {
    this._createProfileDialog.create()
      .pipe(
        map(x => this.addOrUpdate(x)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public addOrUpdate(profile: Profile) {
    if (!profile) return;

    let profiles = [...this.profiles$.value];
    const i = profiles.findIndex((t) => t.profileId == profile.profileId);

    if (i < 0) {
      profiles.push(profile);
    } else {
      profiles[i] = profile;
    }

    this.profiles$.next(profiles);
  }

  public handleRemove($event) {
    const profile = $event.data;

    const cards: Array<Profile> = [...this.profiles$.value];
    const index = cards.findIndex(x => x.profileId == $event.data.profileId);
    cards.splice(index, 1);
    this.profiles$.next(cards);

    this._profileService.remove({ profile: profile })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  private readonly _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }

  public frameworkComponents: any = {
    deleteRenderer: DeleteCellComponent
  };

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name" },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemove($event), width: 30 }
  ];

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}

