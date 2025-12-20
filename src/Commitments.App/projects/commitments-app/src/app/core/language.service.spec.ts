// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from './language.service';
import { LocalStorageService } from './local-storage.service';

describe('LanguageService', () => {
  let service: LanguageService;
  let translateServiceMock: jest.Mocked<TranslateService>;
  let localStorageServiceMock: jest.Mocked<LocalStorageService>;

  beforeEach(() => {
    translateServiceMock = {
      use: jest.fn(),
      getBrowserLang: jest.fn().mockReturnValue('en')
    } as any;

    localStorageServiceMock = {
      get: jest.fn().mockReturnValue(null),
      put: jest.fn()
    } as any;

    TestBed.configureTestingModule({
      providers: [
        LanguageService,
        { provide: TranslateService, useValue: translateServiceMock },
        { provide: LocalStorageService, useValue: localStorageServiceMock }
      ]
    });

    service = TestBed.inject(LanguageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have default language as en', () => {
    expect(service.default).toBe('en');
  });

  it('should return current language', () => {
    expect(service.current).toBeDefined();
  });

  it('should set current language and persist to storage', () => {
    service.setCurrent('fr');
    expect(localStorageServiceMock.put).toHaveBeenCalled();
    expect(translateServiceMock.use).toHaveBeenCalledWith('fr');
  });
});
