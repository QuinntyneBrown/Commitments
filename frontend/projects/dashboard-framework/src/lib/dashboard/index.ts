import { Provider } from '@angular/core';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { DashboardModeService } from './dashboard-mode.service';
import { LayoutPersistenceService } from './layout-persistence.service';

export { DashboardLayoutStore };
export { DashboardModeService };
export { LayoutPersistenceService };
export type * from './dashboard.model';
export * from './dashboard-grid/dashboard-grid.component';
export * from './dashboard-shell/dashboard-shell.component';
export * from './review-scrubber/review-scrubber.component';

export function provideDashboard(): Provider[] {
  return [
    DashboardLayoutStore,
    DashboardModeService,
    LayoutPersistenceService
  ];
}
