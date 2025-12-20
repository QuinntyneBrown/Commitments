// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

import { Login } from './users/login/login';
import { MasterPage } from './master-page/master-page';
import { AnonymousMasterPage } from './anonymous-master-page/anonymous-master-page';
import { TagsPage } from './tags/tags-page/tags-page';
import { NotesPage } from './notes/notes-page/notes-page';
import { SettingsPage } from './settings/settings-page/settings-page';
import { HubClientGuard } from './core/hub-client-guard';
import { EditNotePage } from './notes/edit-note-page/edit-note-page';
import { LanguageGuard } from './core/language-guard';
import { AuthGuard } from './core/auth.guard';
import { TagsResolver } from './tags/tags-resolver.service';
import { NoteResolver } from './notes/note-resolver.service';
import { NotesByTagPage } from './notes/notes-by-tag-page/notes-by-tag-page';
import { MyCommimentsPage } from './commitments/my-commiments-page/my-commiments-page';
import { BehavioursPage } from './behaviours/behaviours-page/behaviours-page';
import { DashboardPage } from './dashboards/dashboard-page/dashboard-page';
import { ActivitiesPage } from './activities/activities-page/activities-page';
import { ToDosPage } from './to-dos/to-dos-page/to-dos-page';
import { CardsPage } from './cards/cards-page/cards-page';
import { ProfilesPage } from './profiles/profiles-page/profiles-page';
import { BehaviourTypesPage } from './behaviour-types/behaviour-types-page/behaviour-types-page';
import { CardLayoutsPage } from './card-layouts/card-layouts-page/card-layouts-page';
import { MyProfilePage } from './profiles/my-profile-page/my-profile-page';
import { FrequenciesPage } from './frequencies/frequencies-page/frequencies-page';

export const routes: Routes = [
  {
    path: 'login',
    component: AnonymousMasterPage,
    children: [
      {
        path: '',
        component: Login
      }
    ]
  },
  {
    path: '',
    component: MasterPage,
    canActivate: [AuthGuard, HubClientGuard],
    children: [
      {
        path: '',
        component: DashboardPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'activities',
        component: ActivitiesPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'behaviours',
        component: BehavioursPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'behaviour-types',
        component: BehaviourTypesPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'cards',
        component: CardsPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'card-layouts',
        component: CardLayoutsPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'commitments',
        component: MyCommimentsPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'frequencies',
        component: FrequenciesPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'my-profile',
        component: MyProfilePage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'notes/create',
        component: EditNotePage,
        canActivate: [LanguageGuard],
        resolve: {
          tags: TagsResolver,
          note: NoteResolver
        }
      },
      {
        path: 'notes',
        component: NotesPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'tags/:slug',
        component: NotesByTagPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'notes/:slug',
        component: EditNotePage,
        canActivate: [LanguageGuard],
        resolve: {
          tags: TagsResolver,
          note: NoteResolver
        }
      },
      {
        path: 'profiles',
        component: ProfilesPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'settings',
        component: SettingsPage,
        canActivate: [LanguageGuard]
      },
      {
        path: 'tags',
        component: TagsPage,
        canActivate: [LanguageGuard],
        resolve: {
          tags: TagsResolver
        }
      },
      {
        path: 'to-dos',
        component: ToDosPage,
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
