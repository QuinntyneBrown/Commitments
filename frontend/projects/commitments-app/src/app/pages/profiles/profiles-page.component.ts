// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
import { ProfileService } from '../../services/profile.service';
import { Profile } from '../../models/profile';
import { CreateProfileDialogService } from '../../services/create-profile-dialog.service';
import { DeleteCellComponent } from '../../components/delete-cell/delete-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-profiles-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './profiles-page.component.html',
  styleUrls: ['./profiles-page.component.scss']
})
export class ProfilesPageComponent {
  private readonly _createProfileDialog = inject(CreateProfileDialogService);
  private readonly _profileService = inject(ProfileService);

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

  private _gridApi: GridApi;

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
