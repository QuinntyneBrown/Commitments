// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from '../core/core.module';
import { TagsService } from './tags.service';
import { TagsPageComponent } from './tags-page/tags-page.component';
import { AddTagDialog } from './add-tag-dialog/add-tag-dialog';
import { SharedModule } from '../shared/shared.module';
import { TagsResolver } from './tags-resolver.service';

const declarations = [TagsPageComponent, AddTagDialog];

const providers = [TagsService, TagsResolver];

@NgModule({
  declarations: declarations,
  imports: [CommonModule, HttpClientModule, RouterModule, CoreModule, SharedModule],
  providers
})
export class TagsModule {}

