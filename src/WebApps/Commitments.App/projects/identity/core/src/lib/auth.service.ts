// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, of, ReplaySubject, Subject } from 'rxjs';
import { map, Observable } from 'rxjs';
import { ACCESS_TOKEN_KEY } from './constants';
import { User, UserService } from './models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _userService = inject(UserService);

  public currentUser = new BehaviorSubject({ } as User);

  public authorized$: ReplaySubject<boolean> = new ReplaySubject(1);

  public authenticated$ = new Subject();

  public authorize(): Observable<boolean> {

    this.authorized$.next(localStorage.getItem(ACCESS_TOKEN_KEY) != null);
    
    return this.authorized$;
  }

  public tryToLogin(username:string, password:string): Observable<any> {

    return this._userService.authenticate({ username, password}).pipe(
      map(response => {
        return {
          token: response.token
        }
      })
    );
  }

}


