// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef, Inject, inject } from '@angular/core';
import { CommonModule, AsyncPipe } from '@angular/common';
import { ProfileService } from '../../services/profile.service';
import { AppStore } from '../../app-store';
import { map, switchMap } from 'rxjs';
import { baseUrl } from '../../core/constants';
import { Observable } from 'rxjs';
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
    AsyncPipe,
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

  public ngOnInit() {
    this._profileService.current()
      .pipe(
        map(x => this._appStore.currentProfile$.next(x)),
        switchMap(x => this._appStore.currentProfile$),
        map(x => {
          this._setCustomProperty('--background-image-url', `url(${this._baseUrl}${x.avatarUrl})`);
        })
      )
      .subscribe();

    this._hubClient.messages$
      .pipe(
        map(x => {
          this._appStore.currentProfile$.next(x.profile);
          this._setCustomProperty('--background-image-url', `url(${this._baseUrl}${x.profile.avatarUrl})`);
        })
      )
      .subscribe();
  }

  protected _setCustomProperty(key: string, value: any) {
    this._elementRef.nativeElement.style.setProperty(key, value);
  }

  public get profileName$(): Observable<string> {
    return this._appStore.currentProfile$.pipe(map(x => x.name));
  }

  public onProfileNameClick() {
    this._router.navigateByUrl('/my-profile');
  }
}
