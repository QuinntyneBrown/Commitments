// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { SettingsPage } from './settings-page';
import { LanguageService } from '../../core/language.service';

describe('SettingsPage', () => {
  let languageServiceMock: jest.Mocked<LanguageService>;

  beforeEach(() => {
    languageServiceMock = {
      current: 'en',
      default: 'en',
      setCurrent: jest.fn()
    } as any;
  });

  it('should be defined', () => {
    expect(SettingsPage).toBeDefined();
  });

  it('should have currentLanguage getter that returns language service current', () => {
    const component = new SettingsPage(languageServiceMock);
    expect(component.currentLanguage).toBe('en');
  });

  it('should have currentLanguage setter that calls language service setCurrent', () => {
    const component = new SettingsPage(languageServiceMock);
    component.currentLanguage = 'fr';
    expect(languageServiceMock.setCurrent).toHaveBeenCalledWith('fr');
  });
});
