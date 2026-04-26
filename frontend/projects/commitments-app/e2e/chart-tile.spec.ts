import { test } from '@playwright/test';

import { ChartTilePage } from './pages/chart-tile.page';

test.describe('chart tile', () => {
  let chart: ChartTilePage;

  test.beforeEach(async ({ page }) => {
    chart = new ChartTilePage(page);
    await chart.goto();
  });

  test('renders a real Chart.js canvas with non-zero dimensions', async () => {
    await chart.addChartTile();

    await chart.expectVisible();
    await chart.expectCanvasHasDimensions();
  });
});
