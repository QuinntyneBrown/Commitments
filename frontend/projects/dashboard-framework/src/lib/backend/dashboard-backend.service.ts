import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, InjectionToken, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

export type DashboardBackendParamValue = string | number | boolean | Date | null | undefined;
export type DashboardBackendParams = Record<string, DashboardBackendParamValue>;

export const DASHBOARD_BACKEND_BASE_URL = new InjectionToken<string>('DASHBOARD_BACKEND_BASE_URL', {
  factory: () => ''
});

@Injectable({ providedIn: 'root' })
export class DashboardBackendService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(DASHBOARD_BACKEND_BASE_URL);

  get<T>(path: string, params?: DashboardBackendParams): Promise<T> {
    return firstValueFrom(
      this._client.get<T>(`${this._baseUrl}${path}`, {
        params: this._toHttpParams(params)
      })
    );
  }

  post<T>(path: string, body: unknown): Promise<T> {
    return firstValueFrom(this._client.post<T>(`${this._baseUrl}${path}`, body));
  }

  put<T>(path: string, body: unknown): Promise<T> {
    return firstValueFrom(this._client.put<T>(`${this._baseUrl}${path}`, body));
  }

  delete<T = void>(path: string): Promise<T> {
    return firstValueFrom(this._client.delete<T>(`${this._baseUrl}${path}`));
  }

  private _toHttpParams(params: DashboardBackendParams = {}): HttpParams {
    return Object.entries(params).reduce((httpParams, [key, value]) => {
      if (value == null) return httpParams;
      return httpParams.set(key, value instanceof Date ? value.toISOString() : String(value));
    }, new HttpParams());
  }
}
