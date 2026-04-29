import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { ProfileService } from '../../data/profile.service';
import { Profile } from '../../data/profile';

@Component({
  selector: 'commitments-profiles-page',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './profiles-page.component.html',
})
export class ProfilesPageComponent implements OnInit {
  private readonly _profileService = inject(ProfileService);

  readonly profiles = signal<Profile[]>([]);

  async ngOnInit(): Promise<void> {
    const { profiles } = await this._profileService.list();
    this.profiles.set(profiles);
  }
}
