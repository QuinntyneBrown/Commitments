// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { DashboardCard } from './dashboard-card';

@Injectable({
  providedIn: 'root'
})
export class DashboardCardService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<DashboardCard>> {
    return this._client.get<{ dashboardCards: Array<DashboardCard> }>(`${this._baseUrl}api/1.0/dashboardCard`)
      .pipe(
        map(x => x.dashboardCards)
      );
  }

  public getById(options: { dashboardCardId: string }): Observable<DashboardCard> {
    return this._client.get<{ dashboardCard: DashboardCard }>(`${this._baseUrl}api/1.0/dashboardCard/${options.dashboardCardId}`)
      .pipe(
        map(x => x.dashboardCard)
      );
  }

  public delete(options: { dashboardCard: DashboardCard }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/dashboardCard/${options.dashboardCard.dashboardCardId}`);
  }

  public create(options: { dashboardCard: DashboardCard }): Observable<{ dashboardCardId: string  }> {    
    return this._client.post<{ dashboardCardId: string }>(`${this._baseUrl}api/1.0/dashboardCard`, { dashboardCard: options.dashboardCard });
  }

  public update(options: { dashboardCard: DashboardCard }): Observable<{ dashboardCardId: string }> {    
    return this._client.post<{ dashboardCardId: string }>(`${this._baseUrl}api/1.0/dashboardCard`, { dashboardCard: options.dashboardCard });
  }
}
