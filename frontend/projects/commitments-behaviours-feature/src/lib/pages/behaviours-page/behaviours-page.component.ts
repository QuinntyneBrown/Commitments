import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { BehaviourService } from '../../data/behaviour.service';
import { Behaviour } from '../../data/behaviour';

@Component({
  selector: 'commitments-behaviours-page',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './behaviours-page.component.html',
})
export class BehavioursPageComponent implements OnInit {
  private readonly _service = inject(BehaviourService);
  readonly behaviours = signal<Behaviour[]>([]);

  async ngOnInit(): Promise<void> {
    const { behaviours } = await this._service.list();
    this.behaviours.set(behaviours);
  }
}
