import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class NotesPagePo extends BasePage {
  readonly url = '/notes';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
