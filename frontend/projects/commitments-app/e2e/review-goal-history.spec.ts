import { expect, test } from '@playwright/test';

import { ReviewGoalHistoryPage } from './pages/review-goal-history.page';

test.describe('review goal history tile', () => {
  let review: ReviewGoalHistoryPage;

  test.beforeEach(async ({ page }) => {
    review = new ReviewGoalHistoryPage(page);
    await review.goto();
  });

  test('renders the polished snapshot tile (badge, metric, delta) sourced from the dashboard scrubber', async () => {
    await review.addReviewGoalHistoryTile();

    await review.expectVisible();
    await expect(review.dateBadge).toBeVisible();
    await expect(review.count).toHaveText('0');
    await expect(review.target).toHaveText('0');
  });
});
