import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class NotesPagePo extends BasePage {
  get url() { return '/notes'; }
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
