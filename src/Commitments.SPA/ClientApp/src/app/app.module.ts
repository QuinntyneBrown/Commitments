import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { baseUrl } from './core/constants';
import { AppRoutingModule } from './app-routing.module';
import { NotesModule } from './notes/notes.module';
import { CoreModule } from './core/core.module';
import { UsersModule } from './users/users.module';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { MasterPageComponent } from './master-page.component';
import { AgGridModule } from 'ag-grid-angular';
import { TagsModule } from './tags/tags.module';
import { TagsPageComponent } from './tags/tags-page.component';
import { SettingsModule } from './settings/settings.module';
import { SharedModule } from './shared/shared.module';
import { CommitmentsModule } from './commitments/commitments.module';
import { ProfilesModule } from './profiles/profiles.module';
import { FrequenciesModule } from './frequencies/frequencies.module';
import { ActivitiesModule } from './activities/activities.module';
import { DashboardsModule } from './dashboards/dashboards.module';
import { CardsModule } from './cards/cards.module';
import { AchievementsModule } from './achievements/achievements.module';
import { ToDosModule } from './to-dos/to-dos.module';

@NgModule({
  declarations: [AppComponent, AnonymousMasterPageComponent, MasterPageComponent],
  imports: [
    AgGridModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,

    AppRoutingModule,

    AchievementsModule,
    ActivitiesModule,
    CardsModule,
    CommitmentsModule,
    CoreModule,
    DashboardsModule,
    FrequenciesModule,
    NotesModule,
    ProfilesModule,
    SettingsModule,
    SharedModule,
    TagsModule,
    ToDosModule,
    UsersModule
  ],
  providers: [{ provide: baseUrl, useValue: 'http://localhost:9613/' }],
  bootstrap: [AppComponent]
})
export class AppModule {}
