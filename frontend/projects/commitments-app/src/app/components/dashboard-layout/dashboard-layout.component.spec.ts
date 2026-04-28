// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { readFileSync } from 'fs';
import { join } from 'path';

import { DashboardLayoutComponent } from './dashboard-layout.component';

describe('DashboardLayoutComponent', () => {
  it('starts with the sidenav open', () => {
    const component = new DashboardLayoutComponent();
    expect((component as unknown as { sidenavOpen: () => boolean }).sidenavOpen()).toBe(true);
  });

  it('toggles the sidenav signal each time the hamburger is invoked', () => {
    const component = new DashboardLayoutComponent();
    const internal = component as unknown as {
      sidenavOpen: () => boolean;
      toggleSidenav: () => void;
    };

    internal.toggleSidenav();
    expect(internal.sidenavOpen()).toBe(false);

    internal.toggleSidenav();
    expect(internal.sidenavOpen()).toBe(true);
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
