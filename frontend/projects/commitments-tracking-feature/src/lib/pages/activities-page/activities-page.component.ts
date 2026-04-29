import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivityService } from '../../data/activity.service';
import { Activity } from '../../data/activity';

@Component({
  selector: 'commitments-activities-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './activities-page.component.html',
})
export class ActivitiesPageComponent implements OnInit {
  private readonly _service = inject(ActivityService);
  readonly activities = signal<Activity[]>([]);

  async ngOnInit(): Promise<void> {
    const { activities } = await this._service.list();
    this.activities.set(activities);
  }
}
