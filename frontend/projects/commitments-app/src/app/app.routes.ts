// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';
import { hubClientGuard } from './core/hub-client-guard';
import { languageGuard } from './core/language-guard';
import { tagsResolver } from './services/tags-resolver.service';
import { noteResolver } from './services/note-resolver.service';
import { MasterPageComponent } from './components/master-page/master-page.component';
import { AnonymousMasterPageComponent } from './components/anonymous-master-page/anonymous-master-page.component';
import { LoginPageComponent } from './pages/login/login-page.component';
import { DashboardPageComponent } from './pages/dashboard/dashboard-page.component';
import { ActivitiesPageComponent } from './pages/activities/activities-page.component';
import { BehavioursPageComponent } from './pages/behaviours/behaviours-page.component';
import { BehaviourTypesPageComponent } from './pages/behaviour-types/behaviour-types-page.component';
import { CardsPageComponent } from './pages/cards/cards-page.component';
import { CardLayoutsPageComponent } from './pages/card-layouts/card-layouts-page.component';
import { CommitmentsPageComponent } from './pages/commitments/commitments-page.component';
import { FrequenciesPageComponent } from './pages/frequencies/frequencies-page.component';
import { MyProfilePageComponent } from './pages/my-profile/my-profile-page.component';
import { EditNotePageComponent } from './pages/edit-note/edit-note-page.component';
import { NotesPageComponent } from './pages/notes/notes-page.component';
import { NotesByTagPageComponent } from './pages/notes-by-tag/notes-by-tag-page.component';
import { ProfilesPageComponent } from './pages/profiles/profiles-page.component';
import { SettingsPageComponent } from './pages/settings/settings-page.component';
import { TagsPageComponent } from './pages/tags/tags-page.component';
import { ToDosPageComponent } from './pages/to-dos/to-dos-page.component';

export const routes: Routes = [
  {
    path: 'login',
    component: AnonymousMasterPageComponent,
    children: [
      {
        path: '',
        component: LoginPageComponent
      }
    ]
  },
  {
    path: '',
    component: MasterPageComponent,
    canActivate: [authGuard, hubClientGuard],
    children: [
      {
        path: '',
        component: DashboardPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'activities',
        component: ActivitiesPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'behaviours',
        component: BehavioursPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'behaviour-types',
        component: BehaviourTypesPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'cards',
        component: CardsPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'card-layouts',
        component: CardLayoutsPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'commitments',
        component: CommitmentsPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'frequencies',
        component: FrequenciesPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'my-profile',
        component: MyProfilePageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'notes/create',
        component: EditNotePageComponent,
        canActivate: [languageGuard],
        resolve: {
          tags: tagsResolver,
          note: noteResolver
        }
      },
      {
        path: 'notes',
        component: NotesPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'tags/:slug',
        component: NotesByTagPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'notes/:slug',
        component: EditNotePageComponent,
        canActivate: [languageGuard],
        resolve: {
          tags: tagsResolver,
          note: noteResolver
        }
      },
      {
        path: 'profiles',
        component: ProfilesPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'settings',
        component: SettingsPageComponent,
        canActivate: [languageGuard]
      },
      {
        path: 'tags',
        component: TagsPageComponent,
        canActivate: [languageGuard],
        resolve: {
          tags: tagsResolver
        }
      },
      {
        path: 'to-dos',
        component: ToDosPageComponent,
        canActivate: [languageGuard]
      }
    ]
  }
];
