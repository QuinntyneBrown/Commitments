import {
  Routes,
  RouterModule
} from '@angular/router';
import { LoginComponent } from './users/login.component';
import { MasterPageComponent } from './master-page.component';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { NgModule } from '@angular/core';
import { TagsPageComponent } from './tags/tags-page.component';
import { NotesPageComponent } from './notes/notes-page.component';
import { SettingsPageComponent } from './settings/settings-page.component';
import { HubClientGuard } from './core/hub-client-guard';
import { EditNotePageComponent } from './notes/edit-note-page.component';
import { LanguageGuard } from './core/language-guard';
import { AuthGuard } from './core/auth.guard';
import { TagsResolver } from './tags/tags-resolver.service';
import { NoteResolver } from './notes/note-resolver.service';
import { NotesByTagPageComponent } from './notes/notes-by-tag-page.component';
import { MyCommimentsPageComponent } from './commitments/my-commiments-page.component';
import { EditCommitmentPageComponent } from './commitments/edit-commitment-page.component';
import { EditBehaviourPageComponent } from './behaviours/edit-behaviour-page.component';
import { FrequencyPageComponent } from './frequencies/frequency-page.component';
import { DashboardPageComponent } from './dashboards/dashboard-page.component';

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
        path: 'commitments/create',
        component: EditCommitmentPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'behaviours/create',
        component: EditBehaviourPageComponent,
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
        path: 'my-commitments',
        component: MyCommimentsPageComponent,
        canActivate: [LanguageGuard]
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
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
