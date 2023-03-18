// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from "@angular/core";
import { AuthService } from "@identity/core";
import { map,of } from "rxjs";

export function createLoginPageViewModel() {

  const authService = inject(AuthService);
  
  return of("login-page works!").pipe(
    map(message => ({ message }))
  );
};


