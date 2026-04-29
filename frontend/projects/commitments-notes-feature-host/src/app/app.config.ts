import { ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideMockDashboardFramework } from '@commitments/dashboard-framework';
import { routes } from './app.routes';

const noteFixtures = [{ noteId: 1, title: 'Getting Started', slug: 'getting-started', body: '<p>Hello</p>', tags: [] }];
const tagFixtures = [{ tagId: 1, name: 'work', slug: 'work' }];

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideRouter(routes),
    ...provideMockDashboardFramework({
      backendResponses: {
        'api/v1.0/notes':                        { notes: noteFixtures },
        'api/v1.0/tags':                         { tags: tagFixtures },
        'api/v1.0/notes/slug/getting-started':   { note: noteFixtures[0] },
        'api/v1.0/notes/tag/work':               { notes: noteFixtures },
      }
    })
  ]
};
