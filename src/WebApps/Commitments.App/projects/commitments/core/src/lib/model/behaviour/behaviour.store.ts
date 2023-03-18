// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Behaviour } from "./behaviour";
import { BehaviourService } from "./behaviour.service";

export interface BehaviourState {
    behaviours: Behaviour[]
}

const initialBehaviourState = {
    behaviours: []
};

@Injectable({
    providedIn:"root"
})
export class BehaviourStore extends ComponentStore<BehaviourState> {
    private  readonly _behaviourService = inject(BehaviourService);

    constructor() {
        super(initialBehaviourState);        
    }

    readonly save = (behaviour:Behaviour, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = behaviour.behaviourId ? this._behaviourService.update({ behaviour }) : this._behaviourService.create({ behaviour });
        
        const updateFn = behaviour?.behaviourId ? ([response, behaviours]: [any, Behaviour[]]) => this.patchState({

            behaviours: behaviours.map(t => response.behaviour.behaviourId == t.behaviourId ? response.behaviour : t)
        })
        :(([response, behaviours]: [any, Behaviour[]]) => this.patchState({ behaviours: [...behaviours, response.behaviour ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.behaviours)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Behaviour>(
        exhaustMap((behaviour) => this._behaviourService.delete({ behaviour: behaviour }).pipe( 
            withLatestFrom(this.select(x => x.behaviours )),           
            tapResponse(
                ([_, behaviours]) => this.patchState({ behaviours: behaviours.filter(t => t.behaviourId != behaviour.behaviourId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._behaviourService.get().pipe(            
            tapResponse(
                behaviours => this.patchState({ behaviours }),
                noop                
            )
        ))
    );    
}
