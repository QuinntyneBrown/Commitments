import { Component, OnInit, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Commitment } from '@commitments/dashboard-framework';
import { ActivityService } from '../../../data/activity.service';
import { Activity } from '../../../data/activity';

@Component({
  selector: 'commitments-edit-activity-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-activity-dialog.component.html',
  styleUrls: ['./activity-dialog.scss'],
})
export class EditActivityDialogComponent implements OnInit {
  private readonly _service = inject(ActivityService);
  readonly activity = input.required<Activity>();
  readonly commitments = input.required<readonly Commitment[]>();
  readonly closed = output<void>();
  readonly saved = output<void>();
  readonly removed = output<void>();

  protected commitmentId = '';
  protected performedOn = '';

  ngOnInit(): void {
    const a = this.activity();
    this.commitmentId = String(a.commitmentId);
    this.performedOn = a.performedOn;
  }

  protected async save(): Promise<void> {
    const id = Number(this.commitmentId);
    if (!id) return;
    await this._service.update(this.activity().activityId, {
      commitmentId: id,
      performedOn: this.performedOn,
    });
    this.saved.emit();
  }

  protected async remove(): Promise<void> {
    await this._service.remove(this.activity().activityId);
    this.removed.emit();
  }

  protected cancel(): void {
    this.closed.emit();
  }
}
