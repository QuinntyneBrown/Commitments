// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { CardService } from '../../services/card.service';
import { Card } from '../../models/card';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { DashboardCardService } from '../../services/dashboard-card.service';
import { switchMap, map } from 'rxjs';
import { DashboardCard } from '../../models/dashboard-card';

@Component({
  selector: 'app-add-dashboard-cards-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-dashboard-cards-dialog.html',
  styleUrls: ['./add-dashboard-cards-dialog.scss']
})
export class AddDashboardCardsDialogComponent {
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _cardService = inject(CardService);
  private readonly _dashboardCardService = inject(DashboardCardService);

  public dashboardId: number;

  private cards$: Observable<Card[]>;

  public selectedCards: Array<Card> = [];

  ngOnInit() {
    this.cards$ = this._cardService.get();
  }

  public handleCardClick(card: Card) {
    if (this.cardIsSelected(card)) {
      this.selectedCards.splice(this.selectedCards.indexOf(card), 1);
    } else {
      this.selectedCards.push(card);
    }
  }

  public cardIsSelected(card: Card) {
    return this.selectedCards.indexOf(card) > -1;
  }

  public tryToAddDashboardCards() {
    const dashboardCards = [];

    for (let i = 0; i < this.selectedCards.length; i++) {
      const dashboardCard = new DashboardCard();
      dashboardCard.cardId = this.selectedCards[i].cardId;
      dashboardCard.dashboardId = this.dashboardId;
      dashboardCards.push(dashboardCard);
    }

    this._dashboardCardService.saveRange({ dashboardCards })
      .pipe(
        switchMap(x => this._dashboardCardService.getByIds({ dashboardCardIds: x.dashboardCardIds })),
        map(dashboardCards => this._overlay.close(dashboardCards))
      )
      .subscribe();
  }

  public handleCancelClick() { this._overlay.close(); }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() { this.onDestroy.next(); }
}
