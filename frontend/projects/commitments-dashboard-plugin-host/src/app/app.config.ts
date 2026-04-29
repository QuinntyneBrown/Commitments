// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ApplicationConfig } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideDashboardFramework } from '@commitments/dashboard-framework';
import { provideCommitmentsDashboardPlugin } from '@commitments/dashboard-plugin';
import { httpRecorderInterceptor } from './harness/http-recorder.interceptor';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([httpRecorderInterceptor])),
    provideAnimations(),
    provideRouter(routes, withComponentInputBinding()),
    ...provideDashboardFramework(),
    ...provideCommitmentsDashboardPlugin()
  ]
};
