import { Observable } from "rxjs";

export interface ICanDeactivate {
  canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
}

