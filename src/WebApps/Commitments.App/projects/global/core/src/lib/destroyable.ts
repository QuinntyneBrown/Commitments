// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, OnDestroy } from "@angular/core";
import { Subject } from "rxjs";

@Injectable()
export class Destroyable implements OnDestroy {

    protected readonly _destroyed$ = new Subject();


    ngOnDestroy() {
        this._destroyed$.next(null);
        this._destroyed$.complete();
    }
}
