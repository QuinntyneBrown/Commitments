// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from "@angular/core";
import { combineLatest, EMPTY, map, merge, of, startWith, Subject, switchMap } from "rxjs";
import { Card, CardStore } from "../../models";

export function createSelectCardViewModel() {

  //const dialogRef = inject(DialogRef)
  const cardStore = inject(CardStore);

  const cancelSubject = new Subject();

  const saveSubject = new Subject();

  const selectSubject: Subject<Card> = new Subject();

  const actions$ = merge(cancelSubject, saveSubject).pipe(
    startWith(EMPTY)
  );

  return combineLatest([
    of([] as Card[]).pipe(
      switchMap(selectedCards => selectSubject.pipe(
        map(card => {
          selectedCards = [...selectedCards, card];
          return selectedCards;
        }),
        startWith([])
      ))
    ),
    cardStore.state$,
    actions$
  ]) .pipe(
    map(([selectedCards, state ]) => {

      return { 
        cards: state.cards,
        cancel: () => cancelSubject.next(null),
        save: () => saveSubject.next(null),
        cardIsSelected: (card:Card) => selectedCards.some(x => x.name == card.name),
        cardSelect: (card:Card) => selectSubject.next(card)
      }
    })
  );

};


