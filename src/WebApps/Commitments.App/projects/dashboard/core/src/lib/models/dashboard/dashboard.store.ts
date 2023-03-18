// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Dashboard } from "./dashboard";
import { DashboardService } from "./dashboard.service";

export interface DashboardState {
    dashboards: Dashboard[]
}

const initialDashboardState = {
    dashboards: []
};

@Injectable({
    providedIn:"root"
})
export class DashboardStore extends ComponentStore<DashboardState> {
    private  readonly _dashboardService = inject(DashboardService);

    constructor() {
        super(initialDashboardState);        
    }

    readonly save = (dashboard:Dashboard, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = dashboard.dashboardId ? this._dashboardService.update({ dashboard }) : this._dashboardService.create({ dashboard });
        
        const updateFn = dashboard?.dashboardId ? ([response, dashboards]: [any, Dashboard[]]) => this.patchState({

            dashboards: dashboards.map(t => response.dashboard.dashboardId == t.dashboardId ? response.dashboard : t)
        })
        :(([response, dashboards]: [any, Dashboard[]]) => this.patchState({ dashboards: [...dashboards, response.dashboard ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.dashboards)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Dashboard>(
        exhaustMap((dashboard) => this._dashboardService.delete({ dashboard: dashboard }).pipe( 
            withLatestFrom(this.select(x => x.dashboards )),           
            tapResponse(
                ([_, dashboards]) => this.patchState({ dashboards: dashboards.filter(t => t.dashboardId != dashboard.dashboardId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._dashboardService.get().pipe(            
            tapResponse(
                dashboards => this.patchState({ dashboards }),
                noop                
            )
        ))
    );    
}
