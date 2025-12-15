// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Profile } from "./profiles/profile";

@Injectable()
export class AppStore {
  public currentProfile$: BehaviorSubject<Profile> = new BehaviorSubject(<Profile>{});
}

