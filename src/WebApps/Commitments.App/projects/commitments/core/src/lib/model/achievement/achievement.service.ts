// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Achievement } from './achievement';

@Injectable({
  providedIn: 'root'
})
export class AchievementService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Achievement>> {
    return this._client.get<{ achievements: Array<Achievement> }>(`${this._baseUrl}api/1.0/achievement`)
      .pipe(
        map(x => x.achievements)
      );
  }

  public getById(options: { achievementId: string }): Observable<Achievement> {
    return this._client.get<{ achievement: Achievement }>(`${this._baseUrl}api/1.0/achievement/${options.achievementId}`)
      .pipe(
        map(x => x.achievement)
      );
  }

  public delete(options: { achievement: Achievement }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/achievement/${options.achievement.achievementId}`);
  }

  public create(options: { achievement: Achievement }): Observable<{ achievementId: string  }> {    
    return this._client.post<{ achievementId: string }>(`${this._baseUrl}api/1.0/achievement`, { achievement: options.achievement });
  }

  public update(options: { achievement: Achievement }): Observable<{ achievementId: string }> {    
    return this._client.post<{ achievementId: string }>(`${this._baseUrl}api/1.0/achievement`, { achievement: options.achievement });
  }
}
