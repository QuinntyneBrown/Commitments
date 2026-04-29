import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const noteFixtures = [{ noteId: 1, slug: 'getting-started', title: 'Getting Started', body: '<p>Hello</p>' }];
const tagFixtures = [{ tagId: 1, slug: 'work', name: 'Work' }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/notes':                       { notes: noteFixtures },
        'api/v1.0/tags':                        { tags: tagFixtures },
        'api/v1.0/notes/slug/getting-started':  { note: noteFixtures[0] },
        'api/v1.0/notes/tag/work':              { notes: noteFixtures },
      }
    })
  ]
};
