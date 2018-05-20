import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Dashboard } from "./dashboard.model";

@Injectable()
export class DashboardService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public getByCurrentProfile(): Observable<Dashboard> {
    return this._client.get<{ dashboard: Dashboard }>(`${this._baseUrl}api/dashboards/currentProfile`)
      .pipe(
        map(x => x.dashboard)
      );
  }

  public getById(options: { dashboardId: number }): Observable<Dashboard> {
    return this._client.get<{ dashboard: Dashboard }>(`${this._baseUrl}api/dashboards/${options.dashboardId}`)
      .pipe(
        map(x => x.dashboard)
      );
  }

  public remove(options: { dashboard: Dashboard }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/dashboards/${options.dashboard.dashboardId}`);
  }

  public save(options: { dashboard: Dashboard }): Observable<{ dashboardId: number }> {
    return this._client.post<{ dashboardId: number }>(`${this._baseUrl}api/dashboards`, { dashboard: options.dashboard });
  }
}
