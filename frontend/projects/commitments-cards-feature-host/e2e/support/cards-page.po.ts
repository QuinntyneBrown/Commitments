import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class CardsPagePo extends BasePage {
  readonly url = '/cards';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
