// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Commitment } from "./commitment";
import { CommitmentService } from "./commitment.service";

export interface CommitmentState {
    commitments: Commitment[]
}

const initialCommitmentState = {
    commitments: []
};

@Injectable({
    providedIn:"root"
})
export class CommitmentStore extends ComponentStore<CommitmentState> {
    private  readonly _commitmentService = inject(CommitmentService);

    constructor() {
        super(initialCommitmentState);        
    }

    readonly save = (commitment:Commitment, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = commitment.commitmentId ? this._commitmentService.update({ commitment }) : this._commitmentService.create({ commitment });
        
        const updateFn = commitment?.commitmentId ? ([response, commitments]: [any, Commitment[]]) => this.patchState({

            commitments: commitments.map(t => response.commitment.commitmentId == t.commitmentId ? response.commitment : t)
        })
        :(([response, commitments]: [any, Commitment[]]) => this.patchState({ commitments: [...commitments, response.commitment ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.commitments)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Commitment>(
        exhaustMap((commitment) => this._commitmentService.delete({ commitment: commitment }).pipe( 
            withLatestFrom(this.select(x => x.commitments )),           
            tapResponse(
                ([_, commitments]) => this.patchState({ commitments: commitments.filter(t => t.commitmentId != commitment.commitmentId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._commitmentService.get().pipe(            
            tapResponse(
                commitments => this.patchState({ commitments }),
                noop                
            )
        ))
    );    
}
