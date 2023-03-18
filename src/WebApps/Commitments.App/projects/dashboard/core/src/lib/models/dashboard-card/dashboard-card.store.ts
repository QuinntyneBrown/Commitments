// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { DashboardCard } from "./dashboard-card";
import { DashboardCardService } from "./dashboard-card.service";

export interface DashboardCardState {
    dashboardCards: DashboardCard[]
}

const initialDashboardCardState = {
    dashboardCards: []
};

@Injectable({
    providedIn:"root"
})
export class DashboardCardStore extends ComponentStore<DashboardCardState> {
    private  readonly _dashboardCardService = inject(DashboardCardService);

    constructor() {
        super(initialDashboardCardState);        
    }

    readonly save = (dashboardCard:DashboardCard, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = dashboardCard.dashboardCardId ? this._dashboardCardService.update({ dashboardCard }) : this._dashboardCardService.create({ dashboardCard });
        
        const updateFn = dashboardCard?.dashboardCardId ? ([response, dashboardCards]: [any, DashboardCard[]]) => this.patchState({

            dashboardCards: dashboardCards.map(t => response.dashboardCard.dashboardCardId == t.dashboardCardId ? response.dashboardCard : t)
        })
        :(([response, dashboardCards]: [any, DashboardCard[]]) => this.patchState({ dashboardCards: [...dashboardCards, response.dashboardCard ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.dashboardCards)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<DashboardCard>(
        exhaustMap((dashboardCard) => this._dashboardCardService.delete({ dashboardCard: dashboardCard }).pipe( 
            withLatestFrom(this.select(x => x.dashboardCards )),           
            tapResponse(
                ([_, dashboardCards]) => this.patchState({ dashboardCards: dashboardCards.filter(t => t.dashboardCardId != dashboardCard.dashboardCardId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._dashboardCardService.get().pipe(            
            tapResponse(
                dashboardCards => this.patchState({ dashboardCards }),
                noop                
            )
        ))
    );    
}
