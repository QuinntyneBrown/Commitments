import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class BehavioursPagePo extends BasePage {
  readonly url = '/behaviours';
  readonly addButton = this.page.getByRole('button', { name: 'Add Behaviour' });
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
