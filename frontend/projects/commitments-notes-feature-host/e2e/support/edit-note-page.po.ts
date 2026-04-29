import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class EditNotePagePo extends BasePage {
  constructor(page: Page, readonly slug: string) { super(page); }
  get url() { return `/edit-note/${this.slug}`; }
  readonly saveButton = this.page.getByRole('button', { name: 'Save' });
}
