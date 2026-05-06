import { Component, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Commitment } from '@commitments/dashboard-framework';
import { ActivityService } from '../../../data/activity.service';

@Component({
  selector: 'commitments-create-activity-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-activity-dialog.component.html',
  styleUrls: ['./activity-dialog.scss'],
})
export class CreateActivityDialogComponent {
  private readonly _service = inject(ActivityService);
  readonly commitments = input.required<readonly Commitment[]>();
  readonly closed = output<void>();
  readonly recorded = output<void>();

  protected commitmentId = '';
  protected performedOn = '';

  protected async save(): Promise<void> {
    const id = Number(this.commitmentId);
    if (!id || !this.performedOn) return;
    await this._service.record({ commitmentId: id, performedOn: this.performedOn });
    this.recorded.emit();
  }

  protected cancel(): void {
    this.closed.emit();
  }
}
