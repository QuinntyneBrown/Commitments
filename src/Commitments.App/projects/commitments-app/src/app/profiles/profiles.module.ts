// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ProfileService } from './profile.service';
import { ProfilesPage } from './profiles-page';
import { CreateProfileOverlay as CreateProfileOverlayComponent } from './create-profile-overlay';
import { CreateProfileOverlay } from './create-profile-overlay';
import { MyProfilePage } from './my-profile-page';
import { DigitalAssetsModule } from '../digital-assets/digital-assets.module';

const declarations = [
  CreateProfileOverlayComponent,
  ProfilesPage,
  MyProfilePage
];

const providers = [
  ProfileService,
  CreateProfileOverlay
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
  })
export class ProfilesModule { }

