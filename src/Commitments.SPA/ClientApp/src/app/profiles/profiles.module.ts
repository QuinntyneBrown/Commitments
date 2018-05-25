import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ProfileService } from './profile.service';
import { EditProfileOverlayComponent } from './edit-profile-overlay.component';
import { ProfilesPageComponent } from './profiles-page.component';
import { EditProfileOverlay } from './edit-profile-overlay';

const declarations = [
  EditProfileOverlayComponent,
  ProfilesPageComponent
];

const providers = [
  ProfileService,
  EditProfileOverlay
];

const entryComponents = [
  EditProfileOverlayComponent
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class ProfilesModule { }
