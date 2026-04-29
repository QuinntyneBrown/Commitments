// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { Router, UrlTree } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { authGuard } from './auth.guard';

function runGuard(): boolean | UrlTree {
  return TestBed.runInInjectionContext(() =>
    authGuard({} as any, {} as any)
  ) as boolean | UrlTree;
}

describe('authGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [RouterTestingModule] });
    localStorage.clear();
  });

  afterEach(() => localStorage.clear());

  it('returns true when an access token is present', () => {
    localStorage.setItem('accessTokenKey', 'fake-jwt');
    expect(runGuard()).toBe(true);
  });

  it('returns a UrlTree redirecting to /login when no token', () => {
    const result = runGuard();
    expect(result instanceof UrlTree).toBe(true);
    expect(TestBed.inject(Router).serializeUrl(result as UrlTree)).toBe('/login');
  });
});
