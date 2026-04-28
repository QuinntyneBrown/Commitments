// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { ProfileService } from '../../services/profile.service';
import { Profile } from '../../models/profile';
import { tap } from 'rxjs';

@Component({
  selector: 'app-create-profile-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-profile-dialog.html',
  styleUrls: ['./create-profile-dialog.scss']
})
export class CreateProfileDialogComponent {
  private readonly _profileService = inject(ProfileService);
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _destroyRef = inject(DestroyRef);

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
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          profile.profileId = x.profileId;
          this._overlay.close(profile);
        })
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    avatarUrl: new FormControl(null, []),
    username: new FormControl(null, []),
    name: new FormControl(null, []),
    password: new FormControl(null, []),
    confirmPassword: new FormControl(null, [])
  });
}
