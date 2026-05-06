import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommitmentService, Commitment } from '@commitments/dashboard-framework';
import { ActivityService } from '../../data/activity.service';
import { Activity } from '../../data/activity';
import { CreateActivityDialogComponent } from './dialogs/create-activity-dialog.component';
import { EditActivityDialogComponent } from './dialogs/edit-activity-dialog.component';

interface NavItem { readonly label: string; readonly active: boolean; }

@Component({
  selector: 'commitments-activities-page',
  standalone: true,
  imports: [CommonModule, CreateActivityDialogComponent, EditActivityDialogComponent],
  templateUrl: './activities-page.component.html',
  styleUrls: ['./activities-page.component.scss'],
})
export class ActivitiesPageComponent implements OnInit {
  private readonly _activityService = inject(ActivityService);
  private readonly _commitmentService = inject(CommitmentService);

  readonly activities = signal<Activity[]>([]);
  readonly commitments = signal<Commitment[]>([]);
  readonly isCreateOpen = signal(false);
  readonly editing = signal<Activity | null>(null);

  readonly navItems: readonly NavItem[] = [
    { label: 'Dashboard',    active: false },
    { label: 'Commitments',  active: false },
    { label: 'Activities',   active: true  },
    { label: 'Notes',        active: false },
    { label: "To-Do's",      active: false },
    { label: 'Settings',     active: false },
    { label: 'Logout',       active: false },
  ];

  async ngOnInit(): Promise<void> {
    await Promise.all([this.refreshActivities(), this.refreshCommitments()]);
  }

  protected openCreate(): void { this.isCreateOpen.set(true); }
  protected closeCreate(): void { this.isCreateOpen.set(false); }
  protected async onRecorded(): Promise<void> {
    this.closeCreate();
    await this.refreshActivities();
  }

  protected openEdit(activity: Activity): void { this.editing.set(activity); }
  protected closeEdit(): void { this.editing.set(null); }
  protected async onSaved(): Promise<void> {
    this.closeEdit();
    await this.refreshActivities();
  }
  protected async onRemoved(): Promise<void> {
    this.closeEdit();
    await this.refreshActivities();
  }

  private async refreshActivities(): Promise<void> {
    const { activities } = await this._activityService.list();
    this.activities.set(activities);
  }
  private async refreshCommitments(): Promise<void> {
    const { commitments } = await this._commitmentService.list();
    this.commitments.set(commitments);
  }
}
