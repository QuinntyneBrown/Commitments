import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class ToDosPagePo extends BasePage {
  readonly url = '/to-dos';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
