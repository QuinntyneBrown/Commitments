// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { FrequencyType } from "./frequency-type";
import { FrequencyTypeService } from "./frequency-type.service";

export interface FrequencyTypeState {
    frequencyTypes: FrequencyType[]
}

const initialFrequencyTypeState = {
    frequencyTypes: []
};

@Injectable({
    providedIn:"root"
})
export class FrequencyTypeStore extends ComponentStore<FrequencyTypeState> {
    private  readonly _frequencyTypeService = inject(FrequencyTypeService);

    constructor() {
        super(initialFrequencyTypeState);        
    }

    readonly save = (frequencyType:FrequencyType, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = frequencyType.frequencyTypeId ? this._frequencyTypeService.update({ frequencyType }) : this._frequencyTypeService.create({ frequencyType });
        
        const updateFn = frequencyType?.frequencyTypeId ? ([response, frequencyTypes]: [any, FrequencyType[]]) => this.patchState({

            frequencyTypes: frequencyTypes.map(t => response.frequencyType.frequencyTypeId == t.frequencyTypeId ? response.frequencyType : t)
        })
        :(([response, frequencyTypes]: [any, FrequencyType[]]) => this.patchState({ frequencyTypes: [...frequencyTypes, response.frequencyType ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.frequencyTypes)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<FrequencyType>(
        exhaustMap((frequencyType) => this._frequencyTypeService.delete({ frequencyType: frequencyType }).pipe( 
            withLatestFrom(this.select(x => x.frequencyTypes )),           
            tapResponse(
                ([_, frequencyTypes]) => this.patchState({ frequencyTypes: frequencyTypes.filter(t => t.frequencyTypeId != frequencyType.frequencyTypeId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._frequencyTypeService.get().pipe(            
            tapResponse(
                frequencyTypes => this.patchState({ frequencyTypes }),
                noop                
            )
        ))
    );    
}
