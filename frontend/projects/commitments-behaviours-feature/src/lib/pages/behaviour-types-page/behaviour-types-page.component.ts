import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { BehaviourTypeService } from '../../data/behaviour-type.service';
import { BehaviourType } from '../../data/behaviour-type';

@Component({
  selector: 'commitments-behaviour-types-page',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './behaviour-types-page.component.html',
})
export class BehaviourTypesPageComponent implements OnInit {
  private readonly _service = inject(BehaviourTypeService);
  readonly behaviourTypes = signal<BehaviourType[]>([]);

  async ngOnInit(): Promise<void> {
    const { behaviourTypes } = await this._service.list();
    this.behaviourTypes.set(behaviourTypes);
  }
}
