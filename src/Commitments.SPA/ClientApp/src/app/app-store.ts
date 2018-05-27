import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Profile } from "./profiles/profile.model";

@Injectable()
export class AppStore {
  public currentProfile$: BehaviorSubject<Profile> = new BehaviorSubject(<Profile>{});
}
