import { Injectable } from "@angular/core";
import { CanDeactivate } from "@angular/router";
import { Observable } from "rxjs";
import { ICanDeactivate } from "./i-can-deactivate";

@Injectable()
export class CanDeactivateGuard implements CanDeactivate<ICanDeactivate> {  
  canDeactivate(
    component: ICanDeactivate
  ): Observable<boolean> | Promise<boolean> | boolean {
    return component.canDeactivate ? component.canDeactivate() : true;
  }
}
