import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class CommitmentsPagePo extends BasePage {
  readonly url = '/commitments';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
