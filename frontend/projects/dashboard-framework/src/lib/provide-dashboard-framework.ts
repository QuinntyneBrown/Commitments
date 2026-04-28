import { APP_INITIALIZER, Provider } from '@angular/core';

import { DashboardLayoutStore } from './dashboard/dashboard-layout.store';
import { TileRegistrationBootstrapper } from './tile-registration';

export function provideDashboardFramework(): Provider[] {
  return [{
    provide: APP_INITIALIZER,
    useFactory: (
      bootstrapper: TileRegistrationBootstrapper,
      layoutStore: DashboardLayoutStore
    ) => () => {
      bootstrapper.bootstrap();
      layoutStore.hydrate();
    },
    deps: [TileRegistrationBootstrapper, DashboardLayoutStore],
    multi: true
  }];
}
