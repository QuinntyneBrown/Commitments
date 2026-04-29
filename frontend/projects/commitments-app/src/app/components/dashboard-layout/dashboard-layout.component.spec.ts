// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injector, runInInjectionContext } from '@angular/core';
import { Router } from '@angular/router';
import { readFileSync } from 'fs';
import { join } from 'path';

import { ProfileService } from '@commitments/identity-feature';
import { DashboardLayoutComponent } from './dashboard-layout.component';

function makeComponent(opts: { profileService?: Partial<ProfileService>; navigate?: jest.Mock } = {}) {
  const profileService = { current: jest.fn().mockResolvedValue({ profile: { profileId: 1, name: 'Test User', avatarUrl: '' } }), ...opts.profileService };
  const navigate = opts.navigate ?? jest.fn();
  const injector = Injector.create({
    providers: [
      { provide: ProfileService, useValue: profileService },
      { provide: Router, useValue: { navigate } },
    ]
  });
  return { component: runInInjectionContext(injector, () => new DashboardLayoutComponent()), navigate };
}

describe('DashboardLayoutComponent', () => {
  beforeEach(() => localStorage.clear());
  afterEach(() => localStorage.clear());

  it('starts with the sidenav open', () => {
    const { component } = makeComponent();
    expect((component as unknown as { sidenavOpen: () => boolean }).sidenavOpen()).toBe(true);
  });

  it('toggles the sidenav signal each time the hamburger is invoked', () => {
    const { component } = makeComponent();
    const internal = component as unknown as {
      sidenavOpen: () => boolean;
      toggleSidenav: () => void;
    };

    internal.toggleSidenav();
    expect(internal.sidenavOpen()).toBe(false);

    internal.toggleSidenav();
    expect(internal.sidenavOpen()).toBe(true);
  });

  it('profileName starts empty before ngOnInit', () => {
    const { component } = makeComponent();
    expect((component as unknown as { profileName: () => string }).profileName()).toBe('');
  });

  it('loads profile name on ngOnInit', async () => {
    const { component } = makeComponent({
      profileService: { current: jest.fn().mockResolvedValue({ profile: { profileId: 1, name: 'Alice', avatarUrl: '' } }) }
    });
    await (component as unknown as { ngOnInit: () => Promise<void> }).ngOnInit();
    expect((component as unknown as { profileName: () => string }).profileName()).toBe('Alice');
  });

  it('navItems does not contain a Logout route entry (design 50)', () => {
    const { component } = makeComponent();
    const navItems = (component as unknown as { navItems: { routerLink: string }[] }).navItems;
    expect(navItems.some(i => i.routerLink === '/login')).toBe(false);
  });

  it('logout clears the access token from localStorage (design 50)', () => {
    localStorage.setItem('accessTokenKey', 'tok');
    const { component } = makeComponent();
    (component as unknown as { logout: () => void }).logout();
    expect(localStorage.getItem('accessTokenKey')).toBeNull();
  });

  it('logout navigates to /login (design 50)', () => {
    const { component, navigate } = makeComponent();
    (component as unknown as { logout: () => void }).logout();
    expect(navigate).toHaveBeenCalledWith(['/login']);
  });

  it('every var(--cui-*) reference in SCSS includes a fallback hex (bug-110)', () => {
    const scss = readFileSync(
      join(__dirname, 'dashboard-layout.component.scss'),
      'utf8'
    );
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });

  it('font-family declarations reference the --cui-font-display token (bug-174)', () => {
    const scss = readFileSync(
      join(__dirname, 'dashboard-layout.component.scss'),
      'utf8'
    );
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /\bfont-family\s*:\s*Inter\b/.test(line));
    expect(offenders).toEqual([]);
  });

  it('active-sidenav background uses color-mix on --cui-primary (bug-188)', () => {
    const scss = readFileSync(
      join(__dirname, 'dashboard-layout.component.scss'),
      'utf8'
    );
    expect(scss).not.toMatch(/#9FA8DA22/);
    expect(scss).toMatch(/color-mix\(in srgb,\s*var\(\s*--cui-primary[^)]*\)\s*13%,\s*transparent\)/);
  });
});
