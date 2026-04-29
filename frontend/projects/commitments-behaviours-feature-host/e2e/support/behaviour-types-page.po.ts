import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class BehaviourTypesPagePo extends BasePage {
  readonly url = '/behaviour-types';
  readonly rows = this.page.getByRole('row');
  constructor(page: Page) { super(page); }
}
