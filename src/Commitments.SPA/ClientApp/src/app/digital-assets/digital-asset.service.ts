import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { DigitalAsset } from "./digital-asset.model";

@Injectable()
export class DigitalAssetService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
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

  public remove(options: { digitalAsset: DigitalAsset }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/digitalAssets/${options.digitalAsset.digitalAssetId}`);
  }

  public save(options: { digitalAsset: DigitalAsset }): Observable<{ digitalAssetId: number }> {
    return this._client.post<{ digitalAssetId: number }>(`${this._baseUrl}api/digitalAssets`, { digitalAsset: options.digitalAsset });
  }

  public upload(options: { data: FormData }) {    
    return this._client.post<{ digitalAssets: Array<any> }>(`${this._baseUrl}api/digitalAssets/upload`, options.data, { headers: { 'Content-Type': 'multipart/form-data' } });
  }
}
