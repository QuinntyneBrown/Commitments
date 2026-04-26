import { APP_INITIALIZER, Provider } from '@angular/core';
import { TileRegistrationBootstrapper } from '../tile-registration';
import { DashboardLayoutStore } from './dashboard-layout.store';

export function dashboardFrameworkInitializerFactory(
  tileBootstrapper: TileRegistrationBootstrapper,
  layoutStore: DashboardLayoutStore
): () => void {
  return () => {
    tileBootstrapper.bootstrap();
    layoutStore.hydrate();
  };
}

export const DASHBOARD_FRAMEWORK_INITIALIZER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: dashboardFrameworkInitializerFactory,
  deps: [TileRegistrationBootstrapper, DashboardLayoutStore],
  multi: true
};
