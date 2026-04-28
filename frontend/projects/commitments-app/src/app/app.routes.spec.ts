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

  it('dashboard-card base and Poster intermediate are gone (bug-148)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const legacy = ['dashboard-card', 'poster-dashboard-card'];
    for (const dir of legacy) {
      const file = join(__dirname, 'components', dir, `${dir}.component.ts`);
      expect(existsSync(file)).toBe(false);
    }
  });

  it('orphaned dashboard cluster (services, dialogs, models) is gone (bug-149)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const dead = [
      'services/dashboard.service.ts',
      'services/dashboard-card.service.ts',
      'services/add-dashboard-cards-dialog.service.ts',
      'services/dashboard-card-configuration-dialog.service.ts',
      'components/add-dashboard-cards-dialog/add-dashboard-cards-dialog.component.ts',
      'components/dashboard-card-configuration-dialog/dashboard-card-configuration-dialog.component.ts',
      'models/dashboard.ts',
      'models/dashboard-card.ts'
    ];
    for (const rel of dead) {
      expect(existsSync(join(__dirname, rel))).toBe(false);
    }
  });

  it('orphaned AchievementService + Achievement model are gone (bug-150)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const dead = [
      'services/achievement.service.ts',
      'models/achievement.ts'
    ];
    for (const rel of dead) {
      expect(existsSync(join(__dirname, rel))).toBe(false);
    }
  });

  it('unused TileInvalidationService is gone and bindTileMode lost the invalidations$ option (bug-151)', () => {
    const { existsSync, readFileSync } = require('fs');
    const { join } = require('path');
    expect(existsSync(join(__dirname, 'dashboard-tiles', 'tile-invalidation.service.ts'))).toBe(false);
    const bindSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'bind-tile-mode.ts'),
      'utf8'
    );
    expect(bindSrc).not.toMatch(/invalidations\$/);
  });

  it('TileContext no longer declares the unused requestFocus method (bug-152)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const modelSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'tile.model.ts'),
      'utf8'
    );
    expect(modelSrc).not.toMatch(/\brequestFocus\b/);
  });

  it('DashboardLayoutStore drops dead resetLayout / toggleEditMode methods (bug-153)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const storeSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard-layout.store.ts'),
      'utf8'
    );
    expect(storeSrc).not.toMatch(/\bresetLayout\s*\(/);
    expect(storeSrc).not.toMatch(/\btoggleEditMode\s*\(/);
  });

  it('DashboardPage POM drops the unused resetLayoutButton locator (bug-153)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const pomSrc = readFileSync(
      join(__dirname, '..', '..', 'e2e', 'pages', 'dashboard.page.ts'),
      'utf8'
    );
    expect(pomSrc).not.toMatch(/resetLayoutButton/);
  });

  it('TileRegistryService drops the unused listTiles() method (bug-154)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const src = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'tile-registry.service.ts'),
      'utf8'
    );
    expect(src).not.toMatch(/\blistTiles\s*\(/);
  });

  it('DashboardLayoutStore drops dead per-mode liveLayout/reviewLayout signals (bug-155)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const src = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard-layout.store.ts'),
      'utf8'
    );
    expect(src).not.toMatch(/\bliveLayout\b/);
    expect(src).not.toMatch(/\breviewLayout\b/);
  });

  it('TileContext drops dead refresh pipeline — refresh$ + requestRefresh (bug-156)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const modelSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'tile.model.ts'),
      'utf8'
    );
    expect(modelSrc).not.toMatch(/refresh\$/);
    expect(modelSrc).not.toMatch(/\brequestRefresh\b/);
  });

  it('TileContext + DashboardItem drop the dead maximize/restore feature (bug-157)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const tileModel = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'tile.model.ts'),
      'utf8'
    );
    expect(tileModel).not.toMatch(/\bisMaximized\b/);
    expect(tileModel).not.toMatch(/\bmaximize\s*\(/);
    expect(tileModel).not.toMatch(/\brestore\s*\(/);

    const dashModel = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard.model.ts'),
      'utf8'
    );
    expect(dashModel).not.toMatch(/\bmaximized\s*:/);
  });

  it('TileContext drops dead remove() and isEditMode members (bug-158)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const modelSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'tile.model.ts'),
      'utf8'
    );
    expect(modelSrc).not.toMatch(/\bremove\s*\(/);
    expect(modelSrc).not.toMatch(/\bisEditMode\b/);
  });

  it('TileContext drops dead identification fields tileId/instanceId (bug-159)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const modelSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'tile-registration', 'tile.model.ts'),
      'utf8'
    );
    const ctxBlock = modelSrc.match(/export interface TileContext \{[\s\S]*?\}/);
    expect(ctxBlock).not.toBeNull();
    expect(ctxBlock![0]).not.toMatch(/\btileId\b/);
    expect(ctxBlock![0]).not.toMatch(/\binstanceId\b/);
  });

  it('FakeTileRegistry no longer carries the stale listTiles() (bug-160)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const specSrc = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard-layout.store.spec.ts'),
      'utf8'
    );
    expect(specSrc).not.toMatch(/\blistTiles\s*\(/);
  });

  it('dashboard.model.ts no longer exports the dead LAYOUT_STORAGE_KEY constant (bug-161)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const src = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard.model.ts'),
      'utf8'
    );
    // Match the legacy LAYOUT_STORAGE_KEY *export*, not the live/review variants.
    expect(src).not.toMatch(/^export const LAYOUT_STORAGE_KEY\b/m);
  });

  it('5 dead tile-like UI components are gone from commitments-ui (bug-162)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const dead = [
      'card', 'review-tile', 'timeline-scrubber',
      'line-chart-tile', 'real-time-metric-tile'
    ];
    for (const dir of dead) {
      const file = join(
        __dirname, '..', '..', '..', 'commitments-ui', 'src', 'lib', dir, `${dir}.component.ts`
      );
      expect(existsSync(file)).toBe(false);
    }
  });

  it('6 dead form/feedback UI components are gone from commitments-ui (bug-163)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const dead = [
      'button', 'chip', 'text-input',
      'radio-group', 'snackbar', 'dialog-shell'
    ];
    for (const dir of dead) {
      const file = join(
        __dirname, '..', '..', '..', 'commitments-ui', 'src', 'lib', dir, `${dir}.component.ts`
      );
      expect(existsSync(file)).toBe(false);
    }
  });

  it('6 dead shell UI components are gone from commitments-ui (bug-164)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const dead = [
      'app-shell', 'dashboard-tile', 'sidenav',
      'sidenav-item', 'toolbar', 'table-row'
    ];
    for (const dir of dead) {
      const file = join(
        __dirname, '..', '..', '..', 'commitments-ui', 'src', 'lib', dir, `${dir}.component.ts`
      );
      expect(existsSync(file)).toBe(false);
    }
  });

  it('tokens.ts is pruned to only the production-used constants (bug-165)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const src = readFileSync(
      join(__dirname, '..', '..', '..', 'commitments-ui', 'src', 'lib', 'tokens', 'tokens.ts'),
      'utf8'
    );
    // Dead constants no longer declared:
    expect(src).not.toMatch(/\bBG_TOOLBAR\b/);
    expect(src).not.toMatch(/\bSURFACE_TILE\b/);
    expect(src).not.toMatch(/\bTEXT_PRIMARY\b/);
    expect(src).not.toMatch(/\bACCENT_LIVE\b/);
    expect(src).not.toMatch(/^export const (FS_|FW_|SP_|RADIUS_|FONT_)/m);
    // The three live tokens + DashboardMode type are still here:
    expect(src).toMatch(/\bACCENT_CHART\b/);
    expect(src).toMatch(/\bBG_APP\b/);
    expect(src).toMatch(/\bTEXT_MUTED\b/);
    expect(src).toMatch(/\bDashboardMode\b/);
  });

  it('dashboard-framework no longer leaks review-scrubber internals (bug-166)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const src = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'index.ts'),
      'utf8'
    );
    expect(src).not.toMatch(/review-scrubber\.controller/);
    expect(src).not.toMatch(/review-scrubber\.helpers/);
    // The component itself remains exported:
    expect(src).toMatch(/review-scrubber\.component/);
  });

  it('dashboard-framework no longer leaks LayoutPersistenceService (bug-167)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const src = readFileSync(
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'index.ts'),
      'utf8'
    );
    expect(src).not.toMatch(/layout-persistence\.service/);
  });

  it('dashboard-framework.initializer file is gone — inlined into provideDashboardFramework (bug-168)', () => {
    const { existsSync } = require('fs');
    const { join } = require('path');
    const file = join(
      __dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard-framework.initializer.ts'
    );
    expect(existsSync(file)).toBe(false);
  });

  it('DeltaTone type aliases are file-local — no exported (bug-170)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const todos = readFileSync(
      join(__dirname, '..', '..', '..', 'commitments-dashboard-plugin', 'src', 'lib', 'tiles', 'outstanding-todos-tile', 'outstanding-todos.controller.ts'),
      'utf8'
    );
    const goal = readFileSync(
      join(__dirname, '..', '..', '..', 'commitments-dashboard-plugin', 'src', 'lib', 'tiles', 'goal-metrics-tile', 'goal-metrics.controller.ts'),
      'utf8'
    );
    expect(todos).not.toMatch(/^export type DeltaTone\b/m);
    expect(goal).not.toMatch(/^export type DeltaTone\b/m);
  });

  it('login-page font-family declarations reference the --cui-font-display token (bug-175)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const scss = readFileSync(
      join(__dirname, 'pages', 'login', 'login-page', 'login-page.component.scss'),
      'utf8'
    );
    const offenders = scss
      .split(/\r?\n/)
      .map((line: string, i: number) => ({ n: i + 1, line }))
      .filter(({ line }: { line: string }) => /\bfont-family\s*:\s*Inter\b/.test(line));
    expect(offenders).toEqual([]);
  });

  it('--cui-on-accent fallback uses canonical 6-char #FFFFFF across dashboard-framework (bug-178)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const files = [
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard-shell', 'dashboard-shell.component.scss'),
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'add-tile-dialog', 'add-tile-dialog.component.scss'),
      join(__dirname, '..', '..', '..', 'dashboard-framework', 'src', 'lib', 'dashboard', 'dashboard-grid', 'dashboard-grid.component.scss')
    ];
    for (const file of files) {
      const scss = readFileSync(file, 'utf8');
      expect(scss).not.toMatch(/var\(--cui-on-accent,\s*#fff\)/);
    }
  });

  it('_tokens.scss no longer carries legacy non-prefixed token duplicates (bug-179)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const tokens = readFileSync(
      join(__dirname, '..', '..', '..', 'commitments-ui', 'src', 'lib', 'tokens', '_tokens.scss'),
      'utf8'
    );
    const dead = [
      '--bg-app:', '--bg-toolbar:', '--bg-sidebar:',
      '--surface-tile:', '--surface-raised:', '--divider:',
      '--accent-live:', '--accent-review:', '--accent-chart:',
      '--accent-success:',
      '--text-primary:', '--text-secondary:', '--text-muted:',
      '--radius-tile:', '--space-tile-pad:', '--shadow-tile:'
    ];
    for (const decl of dead) {
      expect(tokens).not.toContain(decl);
    }
  });

  it('_tokens.scss no longer declares 30 unused --cui- design tokens (bug-180)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const tokens = readFileSync(
      join(__dirname, '..', '..', '..', 'commitments-ui', 'src', 'lib', 'tokens', '_tokens.scss'),
      'utf8'
    );
    const dead = [
      // Typography scale (none consumed via var())
      '--cui-fs-xs:', '--cui-fs-sm:', '--cui-fs-body:', '--cui-fs-md:',
      '--cui-fs-lg:', '--cui-fs-xl:', '--cui-fs-h3:', '--cui-fs-h2:',
      '--cui-fs-h1:', '--cui-fs-display:',
      '--cui-fw-regular:', '--cui-fw-medium:', '--cui-fw-bold:',
      // Spacing scale (none consumed via var())
      '--cui-sp-1:', '--cui-sp-2:', '--cui-sp-3:', '--cui-sp-4:',
      '--cui-sp-5:', '--cui-sp-6:', '--cui-sp-7:', '--cui-sp-8:',
      '--cui-sp-9:', '--cui-sp-10:',
      // Misc unused
      '--cui-text-on-primary:', '--cui-shadow-raised:',
      '--cui-shadow-floating:', '--cui-toolbar-height:',
      '--cui-sidenav-width:', '--cui-font-body:', '--cui-font-mono:',
      '--cui-focus-ring:'
    ];
    for (const decl of dead) {
      expect(tokens).not.toContain(decl);
    }
  });

  it('styles.scss uses canonical fallback hex and font-display token (bug-181)', () => {
    const { readFileSync } = require('fs');
    const { join } = require('path');
    const styles = readFileSync(
      join(__dirname, '..', 'styles.scss'),
      'utf8'
    );
    expect(styles).not.toMatch(/var\(--cui-warn,\s*#f44336\)/);
    expect(styles).toMatch(/var\(\s*--cui-warn\s*,\s*#F44336\s*\)/);
    expect(styles).not.toMatch(/^\s*font-family\s*:\s*Inter\b/m);
  });
});
