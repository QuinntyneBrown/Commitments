// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from "@angular/core";
import { FormControl, UntypedFormGroup } from "@angular/forms";
import { combineLatest, EMPTY, map,merge,startWith, Subject, switchMap } from "rxjs";
import { AuthService } from "../../auth.service";

export function createLoginViewModel() {

  const authService = inject(AuthService);

  const tryToLoginSubject: Subject<{ username:string, password:string}> = new Subject();

  const actions$ = merge(tryToLoginSubject).pipe(
    switchMap(options => authService.tryToLogin(options.username, options.password)),
    startWith(EMPTY)
  )

  const form = new UntypedFormGroup({
    username: new FormControl(""!, []),
    password: new FormControl(""!, [])
  });

  return combineLatest([
    actions$
  ]).pipe(
    map(_ => {
      return {
        form,
        tryToLogin: (options: { username:string, password:string}) => tryToLoginSubject.next(options)
      }
    })
  )
};


