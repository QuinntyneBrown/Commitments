// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { existsSync } from 'fs';
import { join } from 'path';

const core = join(__dirname, 'core');

const deleted = [
  'store.ts',
  'store.spec.ts',
  'auth.service.ts',
  'auth.service.spec.ts',
  'hub-client.ts',
  'hub-client.spec.ts',
  'hub-retry-policy.ts',
  'hub-retry-policy.spec.ts',
  'message-idempotence-cache.ts',
  'message-idempotence-cache.spec.ts',
  'auth.guard.ts',
  'can-deactivate.guard.ts',
  'hub-client-guard.ts',
  'i-can-deactivate.ts',
  'language.service.ts',
  'language-guard.ts',
  'overlay-ref-provider.ts',
  'overlay-ref-wrapper.ts',
  'deep-copy.ts',
  'error.service.ts',
  'local-storage.service.ts',
  'redirect.service.ts',
];

describe('commitments-app core dead-code removal (design 44)', () => {
  deleted.forEach(file => {
    it(`core/${file} is deleted`, () => {
      expect(existsSync(join(core, file))).toBe(false);
    });
  });
});
