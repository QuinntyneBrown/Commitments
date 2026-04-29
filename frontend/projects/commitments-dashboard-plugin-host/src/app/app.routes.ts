// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { TileHarnessComponent } from './harness/tile-harness.component';
import { TileIndexComponent } from './harness/tile-index.component';

export const routes: Routes = [
  { path: 'tile/:tileId', component: TileHarnessComponent },
  { path: '', redirectTo: 'tile-index', pathMatch: 'full' },
  { path: 'tile-index', component: TileIndexComponent }
];
