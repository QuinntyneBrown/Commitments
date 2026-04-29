import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: '../../projects/commitments-identity-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4310' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [
    ['html', { outputFolder: '../../playwright-identity-host-report' }],
    ['junit', { outputFile: '../../test-results/identity-host-junit.xml' }],
  ],
  webServer: {
    command: 'npm run start:identity-host -- --host 127.0.0.1 --port 4310',
    cwd: '../..',
    url: 'http://127.0.0.1:4310',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
