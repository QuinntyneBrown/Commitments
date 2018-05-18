import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
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
