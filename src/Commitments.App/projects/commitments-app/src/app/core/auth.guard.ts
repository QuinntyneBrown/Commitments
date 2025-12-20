// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { LocalStorageService } from '../core/local-storage.service';
import { Injectable } from '@angular/core';
import { accessTokenKey } from '../core/constants';
import { LoginRedirectService } from './redirect.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private readonly _localStorageService: LocalStorageService,
    private readonly _loginRedirectService: LoginRedirectService
  ) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = this._localStorageService.get({ name: accessTokenKey });

    if (token) return true;

    this._loginRedirectService.lastPath = state.url;
    this._loginRedirectService.redirectToLogin();

    return false;
  }
}

