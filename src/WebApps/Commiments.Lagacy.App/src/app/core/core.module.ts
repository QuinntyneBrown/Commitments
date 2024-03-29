// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { LoggerService } from './logger.service';
import { HeaderInterceptor } from './headers.interceptor';
import { HubClient } from './hub-client';
import { HubClientGuard } from './hub-client-guard';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { LanguageService } from './language.service';
import { LanguageGuard } from './language-guard';
import { AuthGuard } from './auth.guard';
import { AuthService } from './auth.service';
import { LoginRedirectService } from './redirect.service';
import { JwtInterceptor } from './jwt.interceptor';
import { Store } from './store';
import { ErrorService } from './error.service';
import { OverlayRefProvider } from './overlay-ref-provider';

const providers = [
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HeaderInterceptor,
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptor,
    multi: true
  },

  AuthGuard,
  AuthService,
  ErrorService,
  HubClient,
  HubClientGuard,
  LanguageGuard,
  LanguageService,
  LocalStorageService,
  LoginRedirectService,
  LoggerService,
  OverlayRefProvider,
  Store
];

export function TranslateHttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: TranslateHttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers,
  exports: [TranslateModule]
})
export class CoreModule {}

