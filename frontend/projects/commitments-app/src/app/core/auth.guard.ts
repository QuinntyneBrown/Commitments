// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from '@angular/core';
import { CanActivateFn, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey } from './constants';
import { LoginRedirectService } from './redirect.service';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean => {
  const localStorageService = inject(LocalStorageService);
  const loginRedirectService = inject(LoginRedirectService);

  const token = localStorageService.get({ name: accessTokenKey });

  if (token) return true;

  loginRedirectService.lastPath = state.url;
  loginRedirectService.redirectToLogin();

  return false;
};
