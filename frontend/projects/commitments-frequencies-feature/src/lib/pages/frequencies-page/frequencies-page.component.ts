import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FrequencyService } from '../../data/frequency.service';
import { Frequency } from '../../data/frequency';

@Component({
  selector: 'commitments-frequencies-page',
  standalone: true,
  imports: [RouterLink, MatButtonModule],
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
