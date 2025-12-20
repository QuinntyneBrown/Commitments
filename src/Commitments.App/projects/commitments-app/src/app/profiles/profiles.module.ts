// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ProfileService } from './profile.service';
import { ProfilesPageComponent } from 'profiles-page/profiles-page.component';
import { CreateProfileDialog } from 'create-profile-dialog/create-profile-dialog';
import { CreateProfileDialogService } from './create-profile-dialog';
import { MyProfilePageComponent } from 'my-profile-page/my-profile-page.component';
import { DigitalAssetsModule } from '../digital-assets/digital-assets.module';

const declarations = [
  CreateProfileDialog,
  ProfilesPageComponent,
  MyProfilePageComponent
];

const providers = [
  ProfileService,
  CreateProfileDialogService
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

