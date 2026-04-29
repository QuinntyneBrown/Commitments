// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { ProfileService } from '../../../services/profile.service';
import { Profile } from '../../../models/profile';
import { CreateProfileDialogService } from '../../../services/create-profile-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type ProfileTableEvent = { data: Profile };

@Component({
  selector: 'app-profiles-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './profiles-page.component.html',
  styleUrls: ['./profiles-page.component.scss'],
})
export class ProfilesPageComponent {
  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Profile }>;

  private readonly _createProfileDialog = inject(CreateProfileDialogService);
  private readonly _profileService = inject(ProfileService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly profiles = signal<Array<Profile>>([]);
  public columns: DataTableColumn<Profile>[] = [];

  public ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (profile) => profile.name },
      { key: 'delete', header: '', template: this.deleteTpl, width: '40px' },
    ];

    this._profileService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.profiles.set(x)),
      )
      .subscribe();
  }

  public handleFABButtonClick(): void {
    this._createProfileDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }

  public addOrUpdate(profile: Profile): void {
    if (!profile) return;

    this.profiles.update((profiles) => {
      const next = [...profiles];
      const i = next.findIndex((t) => t.profileId == profile.profileId);
      if (i < 0) {
        next.push(profile);
      } else {
        next[i] = profile;
      }
      return next;
    });
  }

  public handleRemove($event: ProfileTableEvent): void {
    const profile: Profile = $event.data;

    this.profiles.update((profiles) => profiles.filter((p) => p.profileId !== profile.profileId));

    this._profileService.remove({ profile }).pipe(takeUntilDestroyed(this._destroyRef)).subscribe();
  }
}
