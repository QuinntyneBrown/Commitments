// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import {
  Routes,
  RouterModule
} from '@angular/router';
import { LoginComponent } from './users/login/login.component';
import { MasterPageComponent } from './master-page/master-page.component';
import { AnonymousMasterPageComponent } from './anonymous-master-page/anonymous-master-page.component';
import { NgModule } from '@angular/core';
import { TagsPageComponent } from 'tags/tags-page/tags-page.component';
import { NotesPageComponent } from 'notes/notes-page/notes-page.component';
import { SettingsPageComponent } from 'settings/settings-page/settings-page.component';
import { HubClientGuard } from './core/hub-client-guard';
import { EditNotePageComponent } from 'notes/edit-note-page/edit-note-page.component';
import { LanguageGuard } from './core/language-guard';
import { AuthGuard } from './core/auth.guard';
import { TagsResolver } from './tags/tags-resolver.service';
import { NoteResolver } from './notes/note-resolver.service';
import { NotesByTagPageComponent } from 'notes/notes-by-tag-page/notes-by-tag-page.component';
import { MyCommimentsPageComponent } from 'commitments/my-commiments-page/my-commiments-page.component';
import { BehavioursPageComponent } from 'behaviours/behaviours-page/behaviours-page.component';
import { DashboardPageComponent } from 'dashboards/dashboard-page/dashboard-page.component';
import { EditFrequencyPageComponent } from 'frequencies/edit-frequency-page/edit-frequency-page.component';
import { ActivitiesPageComponent } from 'activities/activities-page/activities-page.component';
import { ToDosPageComponent } from 'to-dos/to-dos-page/to-dos-page.component';
import { CardsPageComponent } from 'cards/cards-page/cards-page.component';
import { ProfilesPageComponent } from 'profiles/profiles-page/profiles-page.component';
import { BehaviourTypesPageComponent } from 'behaviour-types/behaviour-types-page/behaviour-types-page.component';
import { CardLayoutsPageComponent } from 'card-layouts/card-layouts-page/card-layouts-page.component';
import { MyProfilePageComponent } from 'profiles/my-profile-page/my-profile-page.component';
import { FrequenciesPageComponent } from 'frequencies/frequencies-page/frequencies-page.component';


export const routes: Routes = [
  {
    path: 'login',
    component: AnonymousMasterPageComponent,
    children: [
      {
        path: '',
        component: LoginComponent
      }
    ]
  },
  {
    path: '',
    component: MasterPageComponent,
    canActivate: [AuthGuard, HubClientGuard],
    children: [
      {
        path: '',
        component: DashboardPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'activities',
        component: ActivitiesPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'behaviours',
        component: BehavioursPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'behaviour-types',
        component: BehaviourTypesPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'cards',
        component: CardsPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'card-layouts',
        component: CardLayoutsPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'commitments',
        component: MyCommimentsPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'frequencies',
        component: FrequenciesPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'my-profile',
        component: MyProfilePageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'notes/create',
        component: EditNotePageComponent,
        canActivate: [LanguageGuard],
        resolve: {
          tags: TagsResolver,
          note: NoteResolver
        }
      },
      {
        path: 'notes',
        component: NotesPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'tags/:slug',
        component: NotesByTagPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'notes/:slug',
        component: EditNotePageComponent,
        canActivate: [LanguageGuard],
        resolve: {
          tags: TagsResolver,
          note: NoteResolver
        }
      },
      {
        path: 'profiles',
        component: ProfilesPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'settings',
        component: SettingsPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'tags',
        component: TagsPageComponent,
        canActivate: [LanguageGuard],
        resolve: {
          tags: TagsResolver
        }
      },
      {
        path: 'to-dos',
        component: ToDosPageComponent,
        canActivate: [LanguageGuard]
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}

