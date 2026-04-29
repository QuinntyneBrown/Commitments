import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class ActivitiesPagePo extends BasePage {
  readonly url = '/activities';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
