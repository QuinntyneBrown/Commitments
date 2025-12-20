// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ProfilesModule } from '../profiles/profiles.module';

const declarations = [LoginComponent];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    CoreModule,
    ProfilesModule,
    SharedModule
  ]
})
export class UsersModule {}

