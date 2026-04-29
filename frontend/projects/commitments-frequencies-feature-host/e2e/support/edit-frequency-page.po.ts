import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class EditFrequencyPagePo extends BasePage {
  readonly url: string;
  readonly nameInput = this.page.getByLabel('Frequency');
  readonly save = this.page.getByRole('button', { name: 'Save' });
  constructor(page: Page, frequencyId?: string) {
    super(page);
    this.url = frequencyId ? `/edit-frequency/${frequencyId}` : '/edit-frequency';
  }
}
