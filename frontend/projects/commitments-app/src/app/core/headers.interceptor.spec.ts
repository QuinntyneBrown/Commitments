// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { headerInterceptor } from './headers.interceptor';

describe('headerInterceptor', () => {
  let http: HttpClient;
  let mock: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([headerInterceptor])),
        provideHttpClientTesting(),
      ]
    });
    http = TestBed.inject(HttpClient);
    mock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    mock.verify();
    localStorage.clear();
  });

  it('sets Authorization header from localStorage token', () => {
    localStorage.setItem('accessTokenKey', 'my-jwt');
    http.get('/test').subscribe();
    const req = mock.expectOne('/test');
    expect(req.request.headers.get('Authorization')).toBe('Bearer my-jwt');
    req.flush({});
  });

  it('sends empty bearer when no token present', () => {
    http.get('/test').subscribe();
    const req = mock.expectOne('/test');
    expect(req.request.headers.get('Authorization')).toBe('Bearer ');
    req.flush({});
  });
});
