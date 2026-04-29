import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const profileFixture = { profileId: 1, name: 'Alice', avatarUrl: '' };

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/profiles': { profiles: [profileFixture] },
        'api/v1.0/profiles/current': { profile: profileFixture },
      }
    })
  ]
};
