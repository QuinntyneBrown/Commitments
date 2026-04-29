import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class EditFrequencyPagePo extends BasePage {
  constructor(page: Page, readonly frequencyId?: string) { super(page); }
  get url() { return this.frequencyId ? `/edit-frequency/${this.frequencyId}` : '/edit-frequency'; }
  readonly saveButton = this.page.getByRole('button', { name: 'Save' });
}
