// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { Achievement } from "./achievement";

@Injectable()
export class AchievementService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Achievement>> {
    return this._client.get<{ achievements: Array<Achievement> }>(`${this._baseUrl}api/achievements`)
      .pipe(
        map(x => x.achievements)
      );
  }

  public getById(options: { achievementId: number }): Observable<Achievement> {
    return this._client.get<{ achievement: Achievement }>(`${this._baseUrl}api/achievements/${options.achievementId}`)
      .pipe(
        map(x => x.achievement)
      );
  }

  public remove(options: { achievement: Achievement }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/achievements/${options.achievement.achievementId}`);
  }

  public save(options: { achievement: Achievement }): Observable<{ achievementId: number }> {
    return this._client.post<{ achievementId: number }>(`${this._baseUrl}api/achievements`, { achievement: options.achievement });
  }
}

