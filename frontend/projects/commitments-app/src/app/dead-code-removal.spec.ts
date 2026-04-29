import { existsSync } from 'fs';
import { join } from 'path';

const app = join(__dirname);

const deadComponents = [
  'add-tag-dialog', 'anonymous-master-page', 'auto-complete-chip-list',
  'create-profile-dialog', 'digital-asset-url-input',
  'edit-activity-dialog', 'edit-behaviour-dialog', 'edit-behaviour-type-dialog',
  'edit-card-dialog', 'edit-card-layout-dialog', 'edit-commitment-dialog',
  'edit-frequency-dialog', 'edit-to-do-dialog',
  'frequencies-editor', 'frequency-editor',
  'master-page', 'placeholder-page', 'quill-text-editor',
];

const deadServices = [
  'activity.service.ts', 'behaviour.service.ts', 'behaviour-type.service.ts',
  'card.service.ts', 'card-layout.service.ts', 'commitment.service.ts',
  'create-profile-dialog.service.ts', 'digital-asset.service.ts',
  'edit-activity-dialog.service.ts', 'edit-behaviour-dialog.service.ts',
  'edit-behaviour-type-dialog.service.ts', 'edit-card-dialog.service.ts',
  'edit-card-layout-dialog.service.ts', 'edit-commitment-dialog.service.ts',
  'edit-frequency-dialog.service.ts', 'edit-to-do-dialog.service.ts',
  'frequency.service.ts', 'frequency-type.service.ts',
  'note-resolver.service.ts', 'notes.service.ts', 'profile.service.ts',
  'tags.service.ts', 'tags-resolver.service.ts', 'to-do.service.ts',
];

describe('commitments-app dead-code removal (design 43)', () => {
  it('dead dialog components are gone from components/', () => {
    for (const dir of deadComponents) {
      expect(existsSync(join(app, 'components', dir))).toBe(false);
    }
  });

  it('dead entity services and resolvers are gone from services/', () => {
    for (const file of deadServices) {
      expect(existsSync(join(app, 'services', file))).toBe(false);
    }
  });

  it('local models/ directory is gone', () => {
    expect(existsSync(join(app, 'models'))).toBe(false);
  });

  it('app-store.ts is gone', () => {
    expect(existsSync(join(app, 'app-store.ts'))).toBe(false);
  });
});
