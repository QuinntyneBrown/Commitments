// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Dialog, DialogRef } from "@angular/cdk/dialog";
import { inject } from "@angular/core";
import { AuthService, LoginComponent } from "@identity/core";
import { TranslateService } from "@ngx-translate/core";
import { combineLatest, EMPTY, startWith, switchMap, tap } from "rxjs";

export function createAppViewModel() {

    const authService = inject(AuthService);
    const translateService = inject(TranslateService);
    const dialog = inject(Dialog);

    let dialogRef : DialogRef<LoginComponent, LoginComponent> | undefined;

    translateService.setDefaultLang("en");

    translateService.use(localStorage.getItem("currentLanguage") || "en");

    const unauthorizedResponse$ = authService.authorized$.pipe(
        switchMap(authorized => {
            if(!authorized && dialogRef === undefined) {
                dialogRef = dialog.open(LoginComponent)!;
            }
            return EMPTY;
        }),
        startWith(EMPTY)
    );

    const authenticated$ = authService.authenticated$.pipe(
        tap(_ => {
            dialogRef?.close();
            dialogRef = undefined;
        }),
        startWith(EMPTY)
    );

    return combineLatest([
        unauthorizedResponse$,
        authenticated$        
    ]);
}
