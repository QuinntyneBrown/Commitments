// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-placeholder-page',
  standalone: true,
  imports: [PrimaryHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-primary-header [title]="title()"></app-primary-header>
    <section class="placeholder-page">
      <p>Coming soon.</p>
    </section>
  `,
  styles: [`
    :host { display: block; }
    .placeholder-page { padding: 24px; color: var(--cui-text-secondary); }
  `]
})
export class PlaceholderPageComponent {
  private readonly route = inject(ActivatedRoute);

  protected title(): string {
    const path = this.route.snapshot.url.map(s => s.path).join(' ');
    if (!path) return 'Page';
    return path
      .split(/[-\s]/)
      .filter(Boolean)
      .map(s => s.charAt(0).toUpperCase() + s.slice(1))
      .join(' ');
  }
}
