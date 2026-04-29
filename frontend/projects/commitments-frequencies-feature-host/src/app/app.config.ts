import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const frequencyFixtures = [{ frequencyId: 1, frequency: 3, frequencyTypeId: 1 }];
const frequencyTypeFixtures = [{ frequencyTypeId: 1, name: 'Daily' }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes, withComponentInputBinding()),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/frequencies': { frequencies: frequencyFixtures },
        'api/v1.0/frequencyTypes': { frequencyTypes: frequencyTypeFixtures },
        'api/v1.0/frequencies/1': { frequency: frequencyFixtures[0] },
      }
    })
  ]
};
