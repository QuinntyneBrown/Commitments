// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from "@angular/core";
import { FormControl, FormGroup, UntypedFormGroup } from "@angular/forms";
import { combineLatest, EMPTY, map,merge,of, startWith, Subject } from "rxjs";
import { AuthService } from "../../auth.service";

export function createLoginViewModel() {

  const authService = inject(AuthService);

  const tryToLoginSubject: Subject<{ username:string, password:string}> = new Subject();

  const actions$ = merge(tryToLoginSubject).pipe(
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


