// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

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
});
