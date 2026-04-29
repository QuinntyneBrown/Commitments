import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class SettingsPagePo extends BasePage {
  readonly url = '/settings';
  readonly displayName = this.page.getByTestId('settings-display-name');
  readonly version = this.page.getByTestId('settings-version');
  constructor(page: Page) { super(page); }
}
