// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Dashboard } from './dashboard';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Dashboard>> {
    return this._client.get<{ dashboards: Array<Dashboard> }>(`${this._baseUrl}api/1.0/dashboard`)
      .pipe(
        map(x => x.dashboards)
      );
  }

  public getById(options: { dashboardId: string }): Observable<Dashboard> {
    return this._client.get<{ dashboard: Dashboard }>(`${this._baseUrl}api/1.0/dashboard/${options.dashboardId}`)
      .pipe(
        map(x => x.dashboard)
      );
  }

  public delete(options: { dashboard: Dashboard }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/dashboard/${options.dashboard.dashboardId}`);
  }

  public create(options: { dashboard: Dashboard }): Observable<{ dashboardId: string  }> {    
    return this._client.post<{ dashboardId: string }>(`${this._baseUrl}api/1.0/dashboard`, { dashboard: options.dashboard });
  }

  public update(options: { dashboard: Dashboard }): Observable<{ dashboardId: string }> {    
    return this._client.post<{ dashboardId: string }>(`${this._baseUrl}api/1.0/dashboard`, { dashboard: options.dashboard });
  }
}
