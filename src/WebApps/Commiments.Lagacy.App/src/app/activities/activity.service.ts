// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { Activity } from "./activity";

@Injectable()
export class ActivityService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Activity>> {
    return this._client.get<{ activities: Array<Activity> }>(`${this._baseUrl}api/activities`)
      .pipe(
        map(x => x.activities)
      );
  }

  public getById(options: { activityId: number }): Observable<Activity> {
    return this._client.get<{ activity: Activity }>(`${this._baseUrl}api/activities/${options.activityId}`)
      .pipe(
        map(x => x.activity)
      );
  }

  public remove(options: { activity: Activity }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/activities/${options.activity.activityId}`);
  }

  public save(options: { activity: Activity }): Observable<{ activityId: number }> {
    return this._client.post<{ activityId: number }>(`${this._baseUrl}api/activities`, { activity: options.activity });
  }
}

