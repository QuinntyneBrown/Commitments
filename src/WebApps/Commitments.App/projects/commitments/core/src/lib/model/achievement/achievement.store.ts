// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Achievement } from "./achievement";
import { AchievementService } from "./achievement.service";

export interface AchievementState {
    achievements: Achievement[]
}

const initialAchievementState = {
    achievements: []
};

@Injectable({
    providedIn:"root"
})
export class AchievementStore extends ComponentStore<AchievementState> {
    private  readonly _achievementService = inject(AchievementService);

    constructor() {
        super(initialAchievementState);        
    }

    readonly save = (achievement:Achievement, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = achievement.achievementId ? this._achievementService.update({ achievement }) : this._achievementService.create({ achievement });
        
        const updateFn = achievement?.achievementId ? ([response, achievements]: [any, Achievement[]]) => this.patchState({

            achievements: achievements.map(t => response.achievement.achievementId == t.achievementId ? response.achievement : t)
        })
        :(([response, achievements]: [any, Achievement[]]) => this.patchState({ achievements: [...achievements, response.achievement ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.achievements)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Achievement>(
        exhaustMap((achievement) => this._achievementService.delete({ achievement: achievement }).pipe( 
            withLatestFrom(this.select(x => x.achievements )),           
            tapResponse(
                ([_, achievements]) => this.patchState({ achievements: achievements.filter(t => t.achievementId != achievement.achievementId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._achievementService.get().pipe(            
            tapResponse(
                achievements => this.patchState({ achievements }),
                noop                
            )
        ))
    );    
}
