// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Actvitity } from "./actvitity";
import { ActvitityService } from "./actvitity.service";

export interface ActvitityState {
    actvitities: Actvitity[]
}

const initialActvitityState = {
    actvitities: []
};

@Injectable({
    providedIn:"root"
})
export class ActvitityStore extends ComponentStore<ActvitityState> {
    private  readonly _actvitityService = inject(ActvitityService);

    constructor() {
        super(initialActvitityState);        
    }

    readonly save = (actvitity:Actvitity, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = actvitity.actvitityId ? this._actvitityService.update({ actvitity }) : this._actvitityService.create({ actvitity });
        
        const updateFn = actvitity?.actvitityId ? ([response, actvitities]: [any, Actvitity[]]) => this.patchState({

            actvitities: actvitities.map(t => response.actvitity.actvitityId == t.actvitityId ? response.actvitity : t)
        })
        :(([response, actvitities]: [any, Actvitity[]]) => this.patchState({ actvitities: [...actvitities, response.actvitity ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.actvitities)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Actvitity>(
        exhaustMap((actvitity) => this._actvitityService.delete({ actvitity: actvitity }).pipe( 
            withLatestFrom(this.select(x => x.actvitities )),           
            tapResponse(
                ([_, actvitities]) => this.patchState({ actvitities: actvitities.filter(t => t.actvitityId != actvitity.actvitityId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._actvitityService.get().pipe(            
            tapResponse(
                actvitities => this.patchState({ actvitities }),
                noop                
            )
        ))
    );    
}
