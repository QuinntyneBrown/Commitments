import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { CommitmentFrequency } from "./commitment-frequency.model";

@Injectable()
export class CommitmentFrequencyService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<CommitmentFrequency>> {
    return this._client.get<{ commitmentFrequencies: Array<CommitmentFrequency> }>(`${this._baseUrl}api/commitmentFrequencies`)
      .pipe(
        map(x => x.commitmentFrequencies)
      );
  }

  public getById(options: { commitmentFrequencyId: number }): Observable<CommitmentFrequency> {
    return this._client.get<{ commitmentFrequency: CommitmentFrequency }>(`${this._baseUrl}api/commitmentFrequencies/${options.commitmentFrequencyId}`)
      .pipe(
        map(x => x.commitmentFrequency)
      );
  }

  public remove(options: { commitmentFrequency: CommitmentFrequency }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/commitmentFrequencies/${options.commitmentFrequency.commitmentFrequencyId}`);
  }

  public save(options: { commitmentFrequency: CommitmentFrequency }): Observable<{ commitmentFrequencyId: number }> {
    return this._client.post<{ commitmentFrequencyId: number }>(`${this._baseUrl}api/commitmentFrequencies`, { commitmentFrequency: options.commitmentFrequency });
  }
}
