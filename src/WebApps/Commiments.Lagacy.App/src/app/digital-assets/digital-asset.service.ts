// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { DigitalAsset } from "./digital-asset";

@Injectable()
export class DigitalAssetService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<DigitalAsset>> {
    return this._client.get<{ digitalAssets: Array<DigitalAsset> }>(`${this._baseUrl}api/digitalAssets`)
      .pipe(
        map(x => x.digitalAssets)
      );
  }

  public getById(options: { digitalAssetId: number }): Observable<DigitalAsset> {
    return this._client.get<{ digitalAsset: DigitalAsset }>(`${this._baseUrl}api/digitalAssets/${options.digitalAssetId}`)
      .pipe(
        map(x => x.digitalAsset)
      );
  }
  
  public getByIds(options: { digitalAssetIds: number[] }): Observable<DigitalAsset[]> {
    return this._client.get<{ digitalAssets: DigitalAsset[] }>(`${this._baseUrl}api/digitalAssets/range?${options.digitalAssetIds
      .map(x => `digitalAssetIds=${x}`)
      .join('&')}`)
      .pipe(
        map(x => x.digitalAssets)
      );
  }

  public remove(options: { digitalAsset: DigitalAsset }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/digitalAssets/${options.digitalAsset.digitalAssetId}`);
  }

  public save(options: { digitalAsset: DigitalAsset }): Observable<{ digitalAssetId: number }> {
    return this._client.post<{ digitalAssetId: number }>(`${this._baseUrl}api/digitalAssets`, { digitalAsset: options.digitalAsset });
  }

  public upload(options: { data: FormData }): Observable<{ digitalAssetIds: number[] }> {
    
    return this._client.post<{ digitalAssetIds: number[] }>(`${this._baseUrl}api/digitalAssets/upload`,
      options.data);
  }
}

