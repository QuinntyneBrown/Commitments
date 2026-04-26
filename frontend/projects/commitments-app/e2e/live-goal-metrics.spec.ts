import { expect, test } from '@playwright/test';

import { LiveGoalMetricsPage } from './pages/live-goal-metrics.page';

test.describe('live goal metrics tile', () => {
  let live: LiveGoalMetricsPage;

  test.beforeEach(async ({ page }) => {
    live = new LiveGoalMetricsPage(page);
    await live.goto();
  });

  test('renders with title, live badge, and a count of 0/30 when nothing is achieved yet', async () => {
    await live.addLiveGoalMetricsTile();

    await live.expectVisible();
    await expect(live.count).toHaveText('0');
    await expect(live.target).toHaveText('30');
  });
});
