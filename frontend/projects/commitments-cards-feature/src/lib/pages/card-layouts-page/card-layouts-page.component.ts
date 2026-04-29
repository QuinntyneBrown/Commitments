import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardLayoutService } from '../../data/card-layout.service';
import { CardLayout } from '../../data/card-layout';

@Component({
  selector: 'commitments-card-layouts-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './card-layouts-page.component.html',
})
export class CardLayoutsPageComponent implements OnInit {
  private readonly _service = inject(CardLayoutService);
  readonly cardLayouts = signal<CardLayout[]>([]);

  async ngOnInit(): Promise<void> {
    const { cardLayouts } = await this._service.list();
    this.cardLayouts.set(cardLayouts);
  }
}
