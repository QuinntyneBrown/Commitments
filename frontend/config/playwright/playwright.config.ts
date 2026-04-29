import { defineConfig, devices } from '@playwright/test';

/**
 * See https://playwright.dev/docs/test-configuration.
 */
export default defineConfig({
  testDir: '../../projects/commitments-app/e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  // Dev cap at 2 — higher counts overwhelm the single Angular dev
  // server and time out tablet/xl runs (see frontend/docs/bugs/062).
  workers: process.env.CI ? 1 : 2,
  reporter: [
    ['html'],
    ['junit', { outputFile: '../../test-results/junit.xml' }],
    ['json', { outputFile: '../../test-results/results.json' }],
  ],
  use: {
    baseURL: 'http://127.0.0.1:4200',
    ignoreHTTPSErrors: true,
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },

  projects: [
    {
      name: 'tablet',
      use: { ...devices['Desktop Chrome'], viewport: { width: 768, height: 1024 } },
    },
    {
      name: 'lg-desktop',
      use: { ...devices['Desktop Chrome'], viewport: { width: 1280, height: 800 } },
    },
    {
      name: 'xl-desktop',
      use: { ...devices['Desktop Chrome'], viewport: { width: 1920, height: 1080 } },
    },
  ],

  webServer: {
    command: 'npm run start -- --host 127.0.0.1 --port 4200',
    cwd: '../..',
    url: 'http://127.0.0.1:4200',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
});
