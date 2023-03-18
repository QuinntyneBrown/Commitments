// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ProfileService } from './profile.service';
import { ProfilesPageComponent } from './profiles-page.component';
import { CreateProfileOverlayComponent } from './create-profile-overlay.component';
import { CreateProfileOverlay } from './create-profile-overlay';
import { MyProfilePageComponent } from './my-profile-page.component';
import { DigitalAssetsModule } from '../digital-assets/digital-assets.module';

const declarations = [
  CreateProfileOverlayComponent,
  ProfilesPageComponent,
  MyProfilePageComponent
];

const providers = [
  ProfileService,
  CreateProfileOverlay
];

const entryComponents = [
  CreateProfileOverlayComponent
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    DigitalAssetsModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class ProfilesModule { }

