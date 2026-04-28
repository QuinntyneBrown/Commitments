// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, Inject, inject } from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { forkJoin, tap } from 'rxjs';
import { ProfileService } from '../../../services/profile.service';
import { Profile } from '../../../models/profile';
import { baseUrl } from '../../../core/constants';
import { DigitalAssetInputUrlComponent } from '../../../components/digital-asset-url-input/digital-asset-url-input.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-my-profile-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    DigitalAssetInputUrlComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './my-profile-page.component.html',
  styleUrls: ['./my-profile-page.component.scss']
})
export class MyProfilePageComponent {
  private readonly _profileService = inject(ProfileService);
  private readonly _destroyRef = inject(DestroyRef);

  @Inject(baseUrl) public baseUrl: string;

  public readonly profile = toSignal<Profile>(
    this._profileService.current().pipe(
      tap(x => this.form.patchValue({
        name: x.name,
        avatarUrl: x.avatarUrl
      }))
    )
  );

  public handleSaveClick() {
    forkJoin([
      this._profileService.saveAvatarUrl({ avatarUrl: this.form.value.avatarUrl }),
      this._profileService.updateDisplayName({ displayName: this.form.value.name })
    ])
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, []),
    avatarUrl: new FormControl(null, [])
  });
}
