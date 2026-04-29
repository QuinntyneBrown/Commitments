// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { jwtInterceptor } from './jwt.interceptor';

describe('jwtInterceptor', () => {
  let http: HttpClient;
  let mock: HttpTestingController;
  let router: Router;

  beforeEach(() => {
    localStorage.setItem('accessTokenKey', 'some-token');
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        provideHttpClient(withInterceptors([jwtInterceptor])),
        provideHttpClientTesting(),
      ]
    });
    http = TestBed.inject(HttpClient);
    mock = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router);
  });

  afterEach(() => {
    mock.verify();
    localStorage.clear();
  });

  it('clears the token and navigates to /login on 401', () => {
    const navigateSpy = jest.spyOn(router, 'navigate');
    http.get('/protected').subscribe({ error: () => {} });
    mock.expectOne('/protected').flush({ message: 'Unauthorized' }, { status: 401, statusText: 'Unauthorized' });
    expect(localStorage.getItem('accessTokenKey')).toBeNull();
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });

  it('does not clear token or navigate on non-401 errors', () => {
    const navigateSpy = jest.spyOn(router, 'navigate');
    http.get('/protected').subscribe({ error: () => {} });
    mock.expectOne('/protected').flush({ message: 'Server Error' }, { status: 500, statusText: 'Server Error' });
    expect(localStorage.getItem('accessTokenKey')).toBe('some-token');
    expect(navigateSpy).not.toHaveBeenCalled();
  });
});
