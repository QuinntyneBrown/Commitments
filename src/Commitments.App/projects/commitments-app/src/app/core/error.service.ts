// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs';

@Injectable()
export class ErrorService {
  constructor(private readonly _snackBar: MatSnackBar, private readonly _translateService: TranslateService) {}

  public translations$(values: Array<string>): Observable<any> {
    return this._translateService.get(values);
  }

  public handle$(
    httpErrorResponse: HttpErrorResponse,
    message: string = 'Error',
    action: string = 'An error ocurr.Try it again.'
  ) {
    return this.translations$([message, action]).pipe(
      map(translations =>
        this._snackBar.open(translations[message], translations[action], {
          duration: 0
        })
      )
    );
  }
}

