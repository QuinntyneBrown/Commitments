import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { FrequencyService } from '../../data/frequency.service';
import { Frequency } from '../../data/frequency';

@Component({
  selector: 'commitments-frequencies-page',
  standalone: true,
  imports: [CommonModule, MatButtonModule, RouterLink],
  templateUrl: './frequencies-page.component.html',
})
export class FrequenciesPageComponent implements OnInit {
  private readonly _service = inject(FrequencyService);
  readonly frequencies = signal<Frequency[]>([]);

  async ngOnInit(): Promise<void> {
    const { frequencies } = await this._service.list();
    this.frequencies.set(frequencies);
  }
}
