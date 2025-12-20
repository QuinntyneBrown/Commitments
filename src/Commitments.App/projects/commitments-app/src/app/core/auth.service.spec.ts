// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { LocalStorageService } from './local-storage.service';
import { HubClient } from './hub-client';
import { baseUrl } from './constants';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let localStorageServiceMock: jest.Mocked<LocalStorageService>;
  let hubClientMock: jest.Mocked<HubClient>;

  beforeEach(() => {
    localStorageServiceMock = {
      get: jest.fn(),
      put: jest.fn()
    } as any;

    hubClientMock = {
      disconnect: jest.fn()
    } as any;

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: LocalStorageService, useValue: localStorageServiceMock },
        { provide: HubClient, useValue: hubClientMock },
        { provide: baseUrl, useValue: 'http://localhost:52748/' }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should make tryToLogin request', () => {
    const credentials = { username: 'test', password: 'password' };

    service.tryToLogin(credentials).subscribe();

    const req = httpMock.expectOne('http://localhost:52748/api/users/token');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(credentials);
    req.flush({ accessToken: 'token123' });
  });

  it('should logout and disconnect hub client', () => {
    service.logout();
    expect(hubClientMock.disconnect).toHaveBeenCalled();
    expect(localStorageServiceMock.put).toHaveBeenCalled();
  });
});
