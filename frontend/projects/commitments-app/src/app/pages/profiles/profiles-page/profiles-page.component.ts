// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { tap } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid-community';
import { ProfileService } from '../../../services/profile.service';
import { Profile } from '../../../models/profile';
import { CreateProfileDialogService } from '../../../services/create-profile-dialog.service';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly profiles = signal<Array<Profile>>([]);

  public ngOnInit() {
    this._profileService.get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => this.profiles.set(x))
      )
      .subscribe();
  }

  public handleFABButtonClick() {
    this._createProfileDialog.create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => this.addOrUpdate(x))
      )
      .subscribe();
  }

  public addOrUpdate(profile: Profile) {
    if (!profile) return;

    this.profiles.update(profiles => {
      const next = [...profiles];
      const i = next.findIndex(t => t.profileId == profile.profileId);
      if (i < 0) {
        next.push(profile);
      } else {
        next[i] = profile;
      }
      return next;
    });
  }

  public handleRemove($event) {
    const profile: Profile = $event.data;

    this.profiles.update(profiles => profiles.filter(p => p.profileId !== profile.profileId));

    this._profileService.remove({ profile })
      .pipe(takeUntilDestroyed(this._destroyRef))
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

  public localeText: any = {};
}
