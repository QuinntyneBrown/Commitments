// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { DashboardShellComponent } from '@commitments/dashboard-framework';

import { routes } from './app.routes';
import { DashboardLayoutComponent } from './components/dashboard-layout/dashboard-layout.component';
import { LoginPageComponent } from './pages/login/login-page/login-page.component';

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

  it.each([
    'activities',
    'behaviours',
    'behaviour-types',
    'commitments',
    'cards',
    'card-layouts',
    'frequencies',
    'notes',
    'profiles',
    'to-dos',
    'my-profile',
    'settings'
  ])('lazy-loads the %s child route under the layout', (path) => {
    const home = routes.find((r) => r.path === '');
    const child = home?.children?.find((c) => c.path === path);
    expect(child).toBeDefined();
    expect(typeof child!.loadComponent).toBe('function');
  });
});
