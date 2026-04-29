import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardService } from '../../data/card.service';
import { Card } from '../../data/card';

@Component({
  selector: 'commitments-cards-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cards-page.component.html',
})
export class CardsPageComponent implements OnInit {
  private readonly _service = inject(CardService);
  readonly cards = signal<Card[]>([]);

  async ngOnInit(): Promise<void> {
    const { cards } = await this._service.list();
    this.cards.set(cards);
  }
}
