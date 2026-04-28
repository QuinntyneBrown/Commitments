// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { DashboardShellComponent } from '@commitments/dashboard-framework';

import { routes } from './app.routes';
import { DashboardLayoutComponent } from './components/dashboard-layout/dashboard-layout.component';
import { PlaceholderPageComponent } from './components/placeholder-page/placeholder-page.component';
import { LoginPageComponent } from './pages/login/login-page/login-page.component';
import { ActivitiesPageComponent } from './pages/activities/activities-page/activities-page.component';
import { ToDosPageComponent } from './pages/to-dos/to-dos-page/to-dos-page.component';
import { NotesPageComponent } from './pages/notes/notes-page/notes-page.component';
import { EditNotePageComponent } from './pages/edit-note/edit-note-page/edit-note-page.component';
import { TagsPageComponent } from './pages/tags/tags-page/tags-page.component';
import { NotesByTagPageComponent } from './pages/notes-by-tag/notes-by-tag-page/notes-by-tag-page.component';
import { CardsPageComponent } from './pages/cards/cards-page/cards-page.component';
import { CardLayoutsPageComponent } from './pages/card-layouts/card-layouts-page/card-layouts-page.component';
import { BehaviourTypesPageComponent } from './pages/behaviour-types/behaviour-types-page/behaviour-types-page.component';
import { BehavioursPageComponent } from './pages/behaviours/behaviours-page/behaviours-page.component';
import { CommitmentsPageComponent } from './pages/commitments/commitments-page/commitments-page.component';
import { EditFrequencyPageComponent } from './pages/edit-frequency/edit-frequency-page/edit-frequency-page.component';
import { FrequenciesPageComponent } from './pages/frequencies/frequencies-page/frequencies-page.component';
import { MyProfilePageComponent } from './pages/my-profile/my-profile-page/my-profile-page.component';
import { ProfilesPageComponent } from './pages/profiles/profiles-page/profiles-page.component';
import { SettingsPageComponent } from './pages/settings/settings-page/settings-page.component';

describe('app.routes', () => {
  it('maps /login to LoginPageComponent', () => {
    const login = routes.find((r) => r.path === 'login');
    expect(login).toBeDefined();
    expect(login!.component).toBe(LoginPageComponent);
  });

  it('wraps the empty path with DashboardLayoutComponent', () => {
    const home = routes.find((r) => r.path === '');
    expect(home).toBeDefined();
    expect(home!.component).toBe(DashboardLayoutComponent);
  });

  it('renders DashboardShellComponent at the index child of the layout', () => {
    const home = routes.find((r) => r.path === '');
    const index = home?.children?.find((c) => c.path === '');
    expect(index).toBeDefined();
    expect(index!.component).toBe(DashboardShellComponent);
  });

  it('contains no PlaceholderPageComponent routes — every page is fully wired (designs 02–17)', () => {
    const home = routes.find((r) => r.path === '');
    home?.children?.forEach(child => {
      if (child.path !== '') expect(child.component).not.toBe(PlaceholderPageComponent);
    });
  });

  it('routes /profiles to ProfilesPageComponent (design 02-Profiles Slice B)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'profiles');
    expect(child).toBeDefined();
    expect(child!.component).toBe(ProfilesPageComponent);
  });

  it('routes /my-profile to MyProfilePageComponent (design 03-My-Profile Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'my-profile');
    expect(child).toBeDefined();
    expect(child!.component).toBe(MyProfilePageComponent);
  });

  it('routes /settings to SettingsPageComponent (design 04-Settings Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'settings');
    expect(child).toBeDefined();
    expect(child!.component).toBe(SettingsPageComponent);
  });

  it('routes /behaviour-types to BehaviourTypesPageComponent (design 05-Behaviour-Types Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'behaviour-types');
    expect(child).toBeDefined();
    expect(child!.component).toBe(BehaviourTypesPageComponent);
  });

  it('routes /behaviours to BehavioursPageComponent (design 06-Behaviours Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'behaviours');
    expect(child).toBeDefined();
    expect(child!.component).toBe(BehavioursPageComponent);
  });

  it('routes /frequencies to FrequenciesPageComponent (design 07-Frequencies Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'frequencies');
    expect(child).toBeDefined();
    expect(child!.component).toBe(FrequenciesPageComponent);
  });

  it('routes /edit-frequency to EditFrequencyPageComponent for create-new (design 08-Edit-Frequency Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'edit-frequency');
    expect(child).toBeDefined();
    expect(child!.component).toBe(EditFrequencyPageComponent);
  });

  it('routes /edit-frequency/:frequencyId to EditFrequencyPageComponent for edit (design 08-Edit-Frequency Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'edit-frequency/:frequencyId');
    expect(child).toBeDefined();
    expect(child!.component).toBe(EditFrequencyPageComponent);
  });

  it('routes /commitments to CommitmentsPageComponent (design 09-Commitments Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'commitments');
    expect(child).toBeDefined();
    expect(child!.component).toBe(CommitmentsPageComponent);
  });

  it('routes /activities to ActivitiesPageComponent (design 10-Activities Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'activities');
    expect(child).toBeDefined();
    expect(child!.component).toBe(ActivitiesPageComponent);
  });

  it('routes /to-dos to ToDosPageComponent (design 11-To-Dos Slice B)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'to-dos');
    expect(child).toBeDefined();
    expect(child!.component).toBe(ToDosPageComponent);
  });

  it('routes /notes to NotesPageComponent (design 12-Notes Slice B)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'notes');
    expect(child).toBeDefined();
    expect(child!.component).toBe(NotesPageComponent);
  });

  it('routes /edit-note/:slug to EditNotePageComponent with the note resolver (design 13-Edit-Note Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'edit-note/:slug');
    expect(child).toBeDefined();
    expect(child!.component).toBe(EditNotePageComponent);
    expect(child!.resolve).toBeDefined();
    expect(child!.resolve!['note']).toBeDefined();
  });

  it('routes /tags to TagsPageComponent (design 14-Tags Slice B)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'tags');
    expect(child).toBeDefined();
    expect(child!.component).toBe(TagsPageComponent);
  });

  it('routes /notes-by-tag/:slug to NotesByTagPageComponent (design 15-Notes-by-Tag Slice C)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'notes-by-tag/:slug');
    expect(child).toBeDefined();
    expect(child!.component).toBe(NotesByTagPageComponent);
  });

  it('routes /cards to CardsPageComponent (design 16-Cards Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'cards');
    expect(child).toBeDefined();
    expect(child!.component).toBe(CardsPageComponent);
  });

  it('routes /card-layouts to CardLayoutsPageComponent (design 17-Card-Layouts Slice A)', () => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === 'card-layouts');
    expect(child).toBeDefined();
    expect(child!.component).toBe(CardLayoutsPageComponent);
  });

  it('legacy DashboardPageComponent has been removed (bug-146)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const legacy = join(
      __dirname,
      'pages',
      'dashboard',
      'dashboard-page',
      'dashboard-page.component.ts'
    );
    expect(existsSync(legacy)).toBe(false);
  });

  it('orphaned cardId-based dashboard-card components are gone (bug-147)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const legacy = [
      'daily-results-dashboard-card',
      'weekly-results-dashboard-card',
      'monthly-results-dashboard-card',
      'to-do-dashboard-card',
      'relations-results-dashboard-card'
    ];
    for (const dir of legacy) {
      const file = join(__dirname, 'components', dir, `${dir}.component.ts`);
      expect(existsSync(file)).toBe(false);
    }
  });
});
