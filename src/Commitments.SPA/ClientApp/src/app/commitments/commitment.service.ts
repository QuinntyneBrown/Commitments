import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Commitment } from "./commitment.model";
import { baseUrl } from "../core/constants";

@Injectable()
export class CommitmentService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public getDailyByCurrentProfile(): Observable<Array<Commitment>> {
    return this._client.get<{ commitments: Array<Commitment> }>(`${this._baseUrl}api/commitments/daily`)
      .pipe(
        map(x => x.commitments)
      );
  }

  public get(): Observable<Array<Commitment>> {
    return this._client.get<{ commitments: Array<Commitment> }>(`${this._baseUrl}api/commitments`)
      .pipe(
        map(x => x.commitments)
      );
  }

  public getById(options: { commitmentId: number }): Observable<Commitment> {
    return this._client.get<{ commitment: Commitment }>(`${this._baseUrl}api/commitments/${options.commitmentId}`)
      .pipe(
        map(x => x.commitment)
      );
  }

  public remove(options: { commitment: Commitment }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/commitments/${options.commitment.commitmentId}`);
  }

  public save(options: { commitment: Commitment }): Observable<{ commitmentId: number }> {
    return this._client.post<{ commitmentId: number }>(`${this._baseUrl}api/commitments`, { commitment: options.commitment });
  }
}
