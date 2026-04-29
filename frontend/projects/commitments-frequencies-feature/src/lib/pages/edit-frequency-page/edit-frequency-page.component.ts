import { Component, inject, signal, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FrequencyService } from '../../data/frequency.service';

@Component({
  selector: 'commitments-edit-frequency-page',
  standalone: true,
  imports: [ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './edit-frequency-page.component.html',
})
export class EditFrequencyPageComponent implements OnInit {
  private readonly _service = inject(FrequencyService);
  private readonly _route = inject(ActivatedRoute);
  private readonly _router = inject(Router);

  readonly form = new FormGroup({ frequency: new FormControl<number>(1) });
  readonly frequencyTypeId = signal<number | string>(1);

  async ngOnInit(): Promise<void> {
    const id = this._route.snapshot.paramMap.get('frequencyId');
    if (id) {
      const { frequency } = await this._service.getById(id);
      this.form.setValue({ frequency: frequency.frequency });
      this.frequencyTypeId.set(frequency.frequencyTypeId);
    }
  }

  async save(): Promise<void> {
    await this._service.save({ frequency: this.form.value.frequency ?? 1, frequencyTypeId: this.frequencyTypeId() });
    this._router.navigate(['/frequencies']);
  }
}
