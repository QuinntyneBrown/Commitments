import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class EditFrequencyPagePo extends BasePage {
  constructor(page: Page, private readonly frequencyId?: string) { super(page); }
  get url() { return this.frequencyId ? `/edit-frequency/${this.frequencyId}` : '/edit-frequency'; }
  readonly nameInput = this.page.getByLabel('Name');
  readonly save = this.page.getByRole('button', { name: 'Save' });
}
