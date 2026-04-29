import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const frequencyFixtures = [{ frequencyId: '11111111-1111-1111-1111-111111111111', frequency: 3, frequencyTypeId: '1' }];
const frequencyTypeFixtures = [{ frequencyTypeId: '1', name: 'Weekly' }];
const oneFrequencyFixture = { frequencyId: '11111111-1111-1111-1111-111111111111', frequency: 3, frequencyTypeId: '1' };

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/frequencies': { frequencies: frequencyFixtures },
        'api/v1.0/frequencyTypes': { frequencyTypes: frequencyTypeFixtures },
        'api/v1.0/frequencies/11111111-1111-1111-1111-111111111111': { frequency: oneFrequencyFixture },
      }
    })
  ]
};
