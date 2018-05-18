import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Profile } from "./profile.model";

@Injectable()
export class ProfileService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public current(): Observable<Profile> {
    return this._client.get<{ profile: Profile }>(`${this._baseUrl}/api/profiles/current`)
      .pipe(
        map(x => x.profile)
      );
  }
}
