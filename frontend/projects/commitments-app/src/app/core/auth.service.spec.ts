// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { AuthService } from './auth.service';
import { HubClient } from './hub-client';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, baseUrl, currentProfileIdKey } from './constants';

describe('AuthService', () => {
  let storage: jest.Mocked<LocalStorageService>;
  let hub: jest.Mocked<HubClient>;
  let service: AuthService;

  beforeEach(() => {
    storage = { put: jest.fn(), get: jest.fn(), remove: jest.fn() } as unknown as jest.Mocked<LocalStorageService>;
    hub = { disconnect: jest.fn() } as unknown as jest.Mocked<HubClient>;

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: LocalStorageService, useValue: storage },
        { provide: HubClient, useValue: hub },
        { provide: baseUrl, useValue: '/' }
      ]
    });

    service = TestBed.inject(AuthService);
  });

  it('logout clears the access token (existing behavior)', () => {
    service.logout();
    expect(storage.put).toHaveBeenCalledWith({ name: accessTokenKey, value: null });
  });

  it('logout clears the current profile id (design 04-Settings Slice B / L2-035)', () => {
    service.logout();
    expect(storage.put).toHaveBeenCalledWith({ name: currentProfileIdKey, value: null });
  });

  it('logout disconnects the realtime hub', () => {
    service.logout();
    expect(hub.disconnect).toHaveBeenCalled();
  });
});
