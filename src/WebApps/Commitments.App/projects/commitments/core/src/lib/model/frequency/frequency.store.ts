// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Frequency } from "./frequency";
import { FrequencyService } from "./frequency.service";

export interface FrequencyState {
    frequencies: Frequency[]
}

const initialFrequencyState = {
    frequencies: []
};

@Injectable({
    providedIn:"root"
})
export class FrequencyStore extends ComponentStore<FrequencyState> {
    private  readonly _frequencyService = inject(FrequencyService);

    constructor() {
        super(initialFrequencyState);        
    }

    readonly save = (frequency:Frequency, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = frequency.frequencyId ? this._frequencyService.update({ frequency }) : this._frequencyService.create({ frequency });
        
        const updateFn = frequency?.frequencyId ? ([response, frequencies]: [any, Frequency[]]) => this.patchState({

            frequencies: frequencies.map(t => response.frequency.frequencyId == t.frequencyId ? response.frequency : t)
        })
        :(([response, frequencies]: [any, Frequency[]]) => this.patchState({ frequencies: [...frequencies, response.frequency ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.frequencies)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Frequency>(
        exhaustMap((frequency) => this._frequencyService.delete({ frequency: frequency }).pipe( 
            withLatestFrom(this.select(x => x.frequencies )),           
            tapResponse(
                ([_, frequencies]) => this.patchState({ frequencies: frequencies.filter(t => t.frequencyId != frequency.frequencyId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._frequencyService.get().pipe(            
            tapResponse(
                frequencies => this.patchState({ frequencies }),
                noop                
            )
        ))
    );    
}
