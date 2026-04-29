import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class MyProfilePagePo extends BasePage {
  readonly url = '/my-profile';

  constructor(page: Page) { super(page); }
}
