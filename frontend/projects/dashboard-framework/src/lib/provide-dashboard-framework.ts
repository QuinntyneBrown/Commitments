import { Provider } from '@angular/core';
import { DASHBOARD_FRAMEWORK_INITIALIZER } from './dashboard';

export function provideDashboardFramework(): Provider[] {
  return [DASHBOARD_FRAMEWORK_INITIALIZER];
}
