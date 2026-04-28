// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, computed, DestroyRef, effect, ElementRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../services/profile.service';
import { AppStore } from '../../app-store';
import { tap } from 'rxjs';
import { baseUrl } from '../../core/constants';
import { Router, RouterModule } from '@angular/router';
import { HubClient } from '../../core/hub-client';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-master-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    TranslateModule
  ],
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.scss']
})
export class MasterPageComponent {
  private readonly _elementRef = inject(ElementRef);
  private readonly _hubClient = inject(HubClient);
  private readonly _profileService = inject(ProfileService);
  private readonly _appStore = inject(AppStore);
  private readonly _router = inject(Router);
  private readonly _baseUrl: string = inject(baseUrl as any);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly profileName = computed(() => this._appStore.currentProfile().name);

  constructor() {
    effect(() => {
      const profile = this._appStore.currentProfile();
      if (profile?.avatarUrl) {
        this._setCustomProperty('--background-image-url', `url(${this._baseUrl}${profile.avatarUrl})`);
      }
    });
  }

  public ngOnInit() {
    this._profileService.current()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => this._appStore.currentProfile.set(x))
      )
      .subscribe();

    this._hubClient.messages$
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => this._appStore.currentProfile.set(x.profile))
      )
      .subscribe();
  }

  protected _setCustomProperty(key: string, value: any) {
    this._elementRef.nativeElement.style.setProperty(key, value);
  }

  public onProfileNameClick() {
    this._router.navigateByUrl('/my-profile');
  }
}
