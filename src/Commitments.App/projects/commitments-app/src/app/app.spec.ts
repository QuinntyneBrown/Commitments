// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { App } from './app';

describe('App', () => {
  let languageServiceMock: any;
  let translateServiceMock: any;

  beforeEach(() => {
    languageServiceMock = {
      default: 'en',
      current: 'en',
      setCurrent: jest.fn()
    };

    translateServiceMock = {
      setDefaultLang: jest.fn(),
      use: jest.fn()
    };
  });

  it('should be defined', () => {
    expect(App).toBeDefined();
  });

  it('should set default language on initialization', () => {
    new App(languageServiceMock, translateServiceMock);
    expect(translateServiceMock.setDefaultLang).toHaveBeenCalledWith('en');
  });

  it('should use current language on initialization', () => {
    new App(languageServiceMock, translateServiceMock);
    expect(translateServiceMock.use).toHaveBeenCalledWith('en');
  });
});
