import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const behaviourTypeFixtures = [{ behaviourTypeId: '1', name: 'Productive' }];
const behaviourFixtures = [{ behaviourId: '1', name: 'Exercise', behaviourTypeId: '1' }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/behaviourTypes': { behaviourTypes: behaviourTypeFixtures },
        'api/v1.0/behaviours': { behaviours: behaviourFixtures },
      }
    })
  ]
};
