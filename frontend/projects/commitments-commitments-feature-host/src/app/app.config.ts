import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const commitmentFixtures = [{ commitmentId: 1, behaviourId: 1, profileId: 1, commitmentFrequencies: [] }];
const behaviourFixtures = [{ behaviourId: 1, name: 'Exercise' }];
const frequencyFixtures = [{ frequencyId: 1, frequency: 3, frequencyTypeId: 1 }];
const frequencyTypeFixtures = [{ frequencyTypeId: 1, name: 'Daily' }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/commitments':    { commitments: commitmentFixtures },
        'api/v1.0/behaviours':     { behaviours: behaviourFixtures },
        'api/v1.0/frequencies':    { frequencies: frequencyFixtures },
        'api/v1.0/frequencyTypes': { frequencyTypes: frequencyTypeFixtures },
      }
    })
  ]
};
