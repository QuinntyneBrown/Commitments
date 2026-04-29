import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '@commitments/identity-feature';

@Component({
  selector: 'commitments-settings-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './settings-page.component.html',
})
export class SettingsPageComponent implements OnInit {
  private readonly _profiles = inject(ProfileService);
  readonly displayName = signal<string>('');

  async ngOnInit(): Promise<void> {
    const { profile } = await this._profiles.current();
    this.displayName.set(profile.name ?? '');
  }
}
