// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ApplicationConfig } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideDashboardFramework } from '@commitments/dashboard-framework';
import { provideCommitmentsDashboardPlugin } from '@commitments/dashboard-plugin';

import { baseUrl, minimumLogLevel } from './core/constants';
import { headerInterceptor } from './core/headers.interceptor';
import { jwtInterceptor } from './core/jwt.interceptor';
import { LogLevel } from './core/logger.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([headerInterceptor, jwtInterceptor])),
    provideAnimations(),
    ...provideDashboardFramework(),
    ...provideCommitmentsDashboardPlugin(),
    { provide: baseUrl, useValue: 'http://localhost:52748/' },
    { provide: minimumLogLevel, useValue: LogLevel.Trace }
  ]
};
