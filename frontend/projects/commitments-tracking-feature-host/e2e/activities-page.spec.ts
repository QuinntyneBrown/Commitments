import { test, expect } from '@playwright/test';
import { ActivitiesPagePo } from './support/activities-page.po';

test('fetches activities list on load', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  const gets = calls.filter(c => c.method === 'get');
  expect(gets).toContainEqual({ method: 'get', path: 'api/v1.0/activities' });
});

test('renders one row per fixture activity', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});

test('renders the activities page heading', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.heading).toHaveText('Activities');
});

test('renders the activities subtitle', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.subtitle).toContainText('Record activities tied to behaviours');
});

test('renders table column headers in design order', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.columnHeaders).toHaveText(['Behaviour', 'Performed On', 'Description', 'Actions']);
});

test('renders edit link on each activity row', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.editLinks).toHaveCount(1);
});

test('renders the app shell with brand and active Activities nav item', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.brand).toHaveText('Commitments');
  await expect(pom.activeNavItem).toHaveText('Activities');
});

test('renders pagination footer', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.pagination).toBeVisible();
});

test('renders a Record Activity floating action button', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.recordActivityFab).toBeVisible();
});

// Create Activity dialog

test('Record Activity dialog is hidden by default', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.createDialog).toBeHidden();
});

test('clicking the FAB opens the Record Activity dialog', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.recordActivityFab.click();
  await expect(pom.createDialog).toBeVisible();
  await expect(pom.createDialogTitle).toHaveText('Record Activity');
});

test('Record Activity dialog has commitment and performed-on fields', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.recordActivityFab.click();
  await expect(pom.createCommitmentField).toBeVisible();
  await expect(pom.createPerformedOnField).toBeVisible();
});

test('cancel closes Record Activity dialog without posting', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.recordActivityFab.click();
  await pom.createCancelButton.click();
  await expect(pom.createDialog).toBeHidden();
  const calls = await pom.backendCalls();
  expect(calls.filter(c => c.method === 'post' && c.path === 'api/v1.0/activities')).toEqual([]);
});

test('saving Record Activity posts to backend and closes dialog', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.recordActivityFab.click();
  await pom.createCommitmentSelect.selectOption('1');
  await pom.createPerformedOnInput.fill('2026-04-29 12:00');
  await pom.createSaveButton.click();
  await expect(pom.createDialog).toBeHidden();
  const calls = await pom.backendCalls();
  const posted = calls.find(c => c.method === 'post' && c.path === 'api/v1.0/activities');
  expect(posted).toBeDefined();
  expect(posted!.body).toEqual({ commitmentId: 1, performedOn: '2026-04-29 12:00' });
});

// Edit Activity dialog

test('Edit Activity dialog is hidden by default', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.editDialog).toBeHidden();
});

test('clicking edit on a row opens the Edit Activity dialog', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.editLinks.first().click();
  await expect(pom.editDialog).toBeVisible();
  await expect(pom.editDialogTitle).toHaveText('Edit Activity');
});

test('Edit Activity dialog prefills selected activity values', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.editLinks.first().click();
  await expect(pom.editCommitmentSelect).toHaveValue('1');
  await expect(pom.editPerformedOnInput).toHaveValue(/2026-04-29/);
});

test('Edit Activity save sends PUT request and closes dialog', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.editLinks.first().click();
  await pom.editSaveButton.click();
  await expect(pom.editDialog).toBeHidden();
  const calls = await pom.backendCalls();
  expect(calls.some(c => c.method === 'put' && c.path === 'api/v1.0/activities/1')).toBe(true);
});

test('Edit Activity delete sends DELETE request and closes dialog', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await pom.editLinks.first().click();
  await pom.editDeleteButton.click();
  await expect(pom.editDialog).toBeHidden();
  const calls = await pom.backendCalls();
  expect(calls.some(c => c.method === 'delete' && c.path === 'api/v1.0/activities/1')).toBe(true);
});
