import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const cardFixtures = [{ cardId: 1, cardLayoutId: 1, name: 'My Card' }];
const cardLayoutFixtures = [{ cardLayoutId: 1, name: 'Default Layout' }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/cards':       { cards: cardFixtures },
        'api/v1.0/cardLayouts': { cardLayouts: cardLayoutFixtures },
      }
    })
  ]
};
