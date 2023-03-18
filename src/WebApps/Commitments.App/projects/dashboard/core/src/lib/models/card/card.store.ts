// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject, Injectable } from "@angular/core";
import { ComponentStore, tapResponse } from "@ngrx/component-store";
import { exhaustMap, map, noop, tap, withLatestFrom } from "rxjs";
import { Card } from "./card";
import { CardService } from "./card.service";

export interface CardState {
    cards: Card[]
}

const initialCardState = {
    cards: []
};

@Injectable({
    providedIn:"root"
})
export class CardStore extends ComponentStore<CardState> {
    private  readonly _cardService = inject(CardService);

    constructor() {
        super(initialCardState);        
    }

    readonly save = (card:Card, nextFn: {(response:any): void} | null = null, errorFn: {(response:any): void} | null = null) => {        
        
        const apiRequest$ = card.cardId ? this._cardService.update({ card }) : this._cardService.create({ card });
        
        const updateFn = card?.cardId ? ([response, cards]: [any, Card[]]) => this.patchState({

            cards: cards.map(t => response.card.cardId == t.cardId ? response.card : t)
        })
        :(([response, cards]: [any, Card[]]) => this.patchState({ cards: [...cards, response.card ]}));
        
        return this.effect<void>(
            exhaustMap(()=> apiRequest$.pipe(
                withLatestFrom(this.select(x => x.cards)),
                tap(updateFn),
                tapResponse(
                    nextFn || noop,
                    errorFn || noop
                )
            )
        ))();
    }

    readonly delete = this.effect<Card>(
        exhaustMap((card) => this._cardService.delete({ card: card }).pipe( 
            withLatestFrom(this.select(x => x.cards )),           
            tapResponse(
                ([_, cards]) => this.patchState({ cards: cards.filter(t => t.cardId != card.cardId )}),
                noop
            )
        ))
    );

    readonly load = this.effect<void>(
        exhaustMap(_ => this._cardService.get().pipe(            
            tapResponse(
                cards => this.patchState({ cards }),
                noop                
            )
        ))
    );    
}
