import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class FrequenciesPagePo extends BasePage {
  readonly url = '/frequencies';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
