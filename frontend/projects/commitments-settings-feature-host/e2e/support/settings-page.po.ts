import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class SettingsPagePo extends BasePage {
  readonly url = '/settings';
  readonly displayName = this.page.getByTestId('settings-display-name');
  constructor(page: Page) { super(page); }
}
