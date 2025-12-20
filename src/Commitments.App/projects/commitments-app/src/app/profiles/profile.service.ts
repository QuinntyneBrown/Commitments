// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { Profile } from "./profile";

@Injectable()
export class ProfileService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public current(): Observable<Profile> {
    return this._client.get<{ profile: Profile }>(`${this._baseUrl}api/profiles/current`)
      .pipe(
        map(x => x.profile)
      );
  }

  public get(): Observable<Array<Profile>> {
    return this._client.get<{ profiles: Array<Profile> }>(`${this._baseUrl}api/profiles`)
      .pipe(
        map(x => x.profiles)
      );
  }

  public getById(options: { profileId: number }): Observable<Profile> {
    return this._client.get<{ profile: Profile }>(`${this._baseUrl}api/profiles/${options.profileId}`)
      .pipe(
        map(x => x.profile)
      );
  }

  public remove(options: { profile: Profile }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/profiles/${options.profile.profileId}`);
  }

  public create(options: { username: string, password: string, confirmPassword: string }): Observable<{ profileId: number }> {
    return this._client.post<{ profileId: number }>(`${this._baseUrl}api/profiles/create`, options);
  }

  public saveAvatarUrl(options: { profileId: number, avatarUrl: string }): Observable<{ profileId: number }> {
    return this._client.post<{ profileId: number }>(`${this._baseUrl}api/profiles/avatar`, options);
  }
}

