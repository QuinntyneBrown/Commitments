// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef, Inject } from '@angular/core';
import { ProfileService } from './profiles/profile.service';
import { AppStore } from './app-store';
import { map, switchMap } from 'rxjs';
import { baseUrl } from './core/constants';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { HubClient } from './core/hub-client';

@Component({
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.scss'],
  selector: 'app-master-page'
})
export class MasterPageComponent {
  constructor(
    @Inject(baseUrl)private readonly _baseUrl:string,
    private readonly _elementRef: ElementRef,
    private readonly _hubClient: HubClient,
    private readonly _profileService: ProfileService,
    private readonly _appStore: AppStore,
    private readonly _router: Router
  ) {

  }

  public ngOnInit() {
    this._profileService.current()
      .pipe(
        map(x => this._appStore.currentProfile$.next(x)),
        switchMap(x => this._appStore.currentProfile$),
        map(x => {
          this._setCustomProperty("--background-image-url", `url(${this._baseUrl}${x.avatarUrl})`);
        })
      )
      .subscribe();

    this._hubClient.messages$
      .pipe(
        map(x => {
          this._appStore.currentProfile$.next(x.profile);
          this._setCustomProperty("--background-image-url", `url(${this._baseUrl}${x.profile.avatarUrl})`);
        })        
      )
      .subscribe();
  }

  protected _setCustomProperty(key: string, value: any) {
    this._elementRef.nativeElement.style.setProperty(key, value)
  }

  public get profileName$(): Observable<string> {
    return this._appStore.currentProfile$.pipe(map(x => x.name));
  }

  public onProfileNameClick() {
    this._router.navigateByUrl("/my-profile");
  }
}

