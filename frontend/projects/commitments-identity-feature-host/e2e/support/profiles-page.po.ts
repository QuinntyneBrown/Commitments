import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class ProfilesPagePo extends BasePage {
  readonly url = '/profiles';
  readonly rows = this.page.getByRole('row');

  constructor(page: Page) { super(page); }
}
