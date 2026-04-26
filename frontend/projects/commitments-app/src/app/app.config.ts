// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';

import { routes } from './app.routes';
import { baseUrl, minimumLogLevel } from './core/constants';
import { headerInterceptor } from './core/headers.interceptor';
import { jwtInterceptor } from './core/jwt.interceptor';
import { LogLevel } from './core/logger.service';
import { AppStore } from './app-store';

export function TranslateHttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([headerInterceptor, jwtInterceptor])),
    provideAnimations(),
    importProvidersFrom(
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: TranslateHttpLoaderFactory,
          deps: [HttpClient]
        }
      })
    ),
    { provide: baseUrl, useValue: 'http://localhost:52748/' },
    { provide: minimumLogLevel, useValue: LogLevel.Trace },
    AppStore
  ]
};
