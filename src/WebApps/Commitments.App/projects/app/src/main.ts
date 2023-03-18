// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { importProvidersFrom } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { RouterModule } from '@angular/router';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BASE_URL } from '@dashboard/core';
import { AuthInterceptor, HeadersInterceptor } from '@identity/core';
import { TranslateLoader } from '@ngx-translate/core';
import { TranslateModule  } from '@ngx-translate/core';
import { TranslateHttpLoader  } from '@ngx-translate/http-loader';

export function HttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}

bootstrapApplication(AppComponent, {
  providers: [
    { provide: BASE_URL, useValue: "https://localhost:55360/" },  
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HeadersInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },      
    importProvidersFrom(
      HttpClientModule,
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
        }
      }),
      HttpClientModule,
      RouterModule.forRoot([
        {
          path: '',
          pathMatch: 'full',
          redirectTo: 'dashboard'
        },
        {
          path: '',
          pathMatch: 'full',
          redirectTo: 'dashboard'
        },
        {
          path: '',
          pathMatch: 'full',
          redirectTo: 'dashboard'
        } 
      ]),     
    )
  ]
}).catch((err) => console.error(err));
