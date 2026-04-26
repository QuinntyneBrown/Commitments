// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { HubClient } from './hub-client';

export const hubClientGuard: CanActivateFn = (): Promise<boolean> => {
  const hubClient = inject(HubClient);
  return new Promise(resolve =>
    hubClient.connect().then(() => {
      resolve(true);
    })
  );
};
