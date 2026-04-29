import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../data/profile.service';
import { Profile } from '../../data/profile';

@Component({
  selector: 'commitments-my-profile-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-profile-page.component.html',
})
export class MyProfilePageComponent implements OnInit {
  private readonly _profileService = inject(ProfileService);

  readonly profile = signal<Profile | null>(null);

  async ngOnInit(): Promise<void> {
    const { profile } = await this._profileService.current();
    this.profile.set(profile);
  }
}
