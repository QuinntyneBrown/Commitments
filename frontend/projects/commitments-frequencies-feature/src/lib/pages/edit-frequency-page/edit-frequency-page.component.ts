import { Component, inject, signal, OnInit, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FrequencyService } from '../../data/frequency.service';
import { Frequency } from '../../data/frequency';

@Component({
  selector: 'commitments-edit-frequency-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './edit-frequency-page.component.html',
})
export class EditFrequencyPageComponent implements OnInit {
  private readonly _service = inject(FrequencyService);

  readonly frequencyId = input<string | undefined>(undefined);
  readonly frequency = signal<Frequency | null>(null);
  readonly form = new FormGroup({ frequency: new FormControl<number | null>(null) });

  async ngOnInit(): Promise<void> {
    const id = this.frequencyId();
    if (id) {
      const { frequency } = await this._service.getById(id);
      this.frequency.set(frequency);
      this.form.patchValue({ frequency: frequency.frequency });
    }
  }

  async save(): Promise<void> {
    const id = this.frequencyId();
    await this._service.save({ ...(id ? { frequencyId: Number(id) } : {}), ...this.form.value });
  }
}
