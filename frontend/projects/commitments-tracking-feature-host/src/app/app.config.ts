import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const activityFixtures = [{ activityId: 1, commitmentId: 1, performedOn: '2026-04-29T00:00:00.000Z' }];
const toDoFixtures = [{ toDoId: 1, commitmentId: 1, dueOn: '2026-04-29T00:00:00.000Z' }];
const commitmentFixtures = [{ commitmentId: 1, behaviourId: 1, profileId: 1, commitmentFrequencies: [] }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/activities':  { activities: activityFixtures },
        'api/v1.0/toDos':       { toDos: toDoFixtures },
        'api/v1.0/commitments': { commitments: commitmentFixtures },
      }
    })
  ]
};
