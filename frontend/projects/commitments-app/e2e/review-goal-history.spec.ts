import { expect, test } from '@playwright/test';

import { ReviewGoalHistoryPage } from './pages/review-goal-history.page';

test.describe('review goal history tile', () => {
  let review: ReviewGoalHistoryPage;

  test.beforeEach(async ({ page }) => {
    review = new ReviewGoalHistoryPage(page);
    await review.goto();
  });

  test('renders with title, review badge, scrubber controls, and a default 0/30 readout', async () => {
    await review.addReviewGoalHistoryTile();

    await review.expectVisible();
    await expect(review.startInput).toBeVisible();
    await expect(review.endInput).toBeVisible();
    await expect(review.applyButton).toBeVisible();
    await expect(review.applyButton).toBeDisabled();
    await expect(review.count).toHaveText('0');
    await expect(review.target).toHaveText('30');
  });
});
