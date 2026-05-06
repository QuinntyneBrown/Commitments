import { Page } from '@playwright/test';
import { BasePage } from './pom-base';
export class ActivitiesPagePo extends BasePage {
  readonly url = '/activities';
  readonly rows = this.page.getByRole('row');
  readonly heading = this.page.getByRole('heading', { level: 1 });
  readonly subtitle = this.page.getByTestId('activities-subtitle');
  readonly columnHeaders = this.page.getByRole('columnheader');
  readonly editLinks = this.page.getByRole('link', { name: /^edit/i });
  readonly brand = this.page.getByTestId('app-brand');
  readonly activeNavItem = this.page.getByTestId('nav-active');
  readonly pagination = this.page.getByTestId('activities-pagination');
  readonly recordActivityFab = this.page.getByRole('button', { name: 'Record Activity' });

  // Create Activity dialog
  readonly createDialog = this.page.getByTestId('create-activity-dialog');
  readonly createDialogTitle = this.page.getByTestId('create-activity-title');
  readonly createCommitmentField = this.page.getByTestId('create-activity-commitment-field');
  readonly createPerformedOnField = this.page.getByTestId('create-activity-performed-on-field');
  readonly createCommitmentSelect = this.page.getByTestId('create-activity-commitment');
  readonly createPerformedOnInput = this.page.getByTestId('create-activity-performed-on');
  readonly createCancelButton = this.page.getByTestId('create-activity-cancel');
  readonly createSaveButton = this.page.getByTestId('create-activity-save');

  // Edit Activity dialog
  readonly editDialog = this.page.getByTestId('edit-activity-dialog');
  readonly editDialogTitle = this.page.getByTestId('edit-activity-title');
  readonly editCommitmentSelect = this.page.getByTestId('edit-activity-commitment');
  readonly editPerformedOnInput = this.page.getByTestId('edit-activity-performed-on');
  readonly editCancelButton = this.page.getByTestId('edit-activity-cancel');
  readonly editSaveButton = this.page.getByTestId('edit-activity-save');
  readonly editDeleteButton = this.page.getByTestId('edit-activity-delete');

  constructor(page: Page) { super(page); }
}
