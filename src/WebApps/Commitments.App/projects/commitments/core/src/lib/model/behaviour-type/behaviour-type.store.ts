// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { BehaviourType } from "./behaviour-type";
import { BehaviourTypeService } from "./behaviour-type.service";

export interface BehaviourTypeState {
    behaviourTypes: BehaviourType[]
}

const initialBehaviourTypeState = {
    behaviourTypes: []
};

@Injectable({
    providedIn:"root"
})
export class BehaviourTypeStore extends ComponentStore<BehaviourTypeState> {
    private  readonly _behaviourTypeService = inject(BehaviourTypeService);

    constructor() {
        super(initialBehaviourTypeState);        
    }

    readonly save = (behaviourType:BehaviourType, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = behaviourType.behaviourTypeId ? this._behaviourTypeService.update({ behaviourType }) : this._behaviourTypeService.create({ behaviourType });
        
        const updateFn = behaviourType?.behaviourTypeId ? ([response, behaviourTypes]: [any, BehaviourType[]]) => this.patchState({

            behaviourTypes: behaviourTypes.map(t => response.behaviourType.behaviourTypeId == t.behaviourTypeId ? response.behaviourType : t)
        })
        :(([response, behaviourTypes]: [any, BehaviourType[]]) => this.patchState({ behaviourTypes: [...behaviourTypes, response.behaviourType ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.behaviourTypes)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<BehaviourType>(
        exhaustMap((behaviourType) => this._behaviourTypeService.delete({ behaviourType: behaviourType }).pipe( 
            withLatestFrom(this.select(x => x.behaviourTypes )),           
            tapResponse(
                ([_, behaviourTypes]) => this.patchState({ behaviourTypes: behaviourTypes.filter(t => t.behaviourTypeId != behaviourType.behaviourTypeId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._behaviourTypeService.get().pipe(            
            tapResponse(
                behaviourTypes => this.patchState({ behaviourTypes }),
                noop                
            )
        ))
    );    
}
