// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-primary-header',
  standalone: true,
  imports: [MatToolbarModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './primary-header.component.html',
  styleUrls: ['./primary-header.component.scss']
})
export class PrimaryHeaderComponent {
  readonly title = input('');
  readonly editMode = input(false);
}
