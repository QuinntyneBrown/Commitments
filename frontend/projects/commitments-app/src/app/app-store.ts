// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, signal, WritableSignal } from '@angular/core';
import { Profile } from './models/profile';

@Injectable({ providedIn: 'root' })
export class AppStore {
  public readonly currentProfile: WritableSignal<Profile> = signal(<Profile>{});
}
