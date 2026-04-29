import { defineConfig, devices } from '@playwright/test';
export default defineConfig({
  testDir: './projects/commitments-commitments-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4340' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [['html', { outputFolder: 'playwright-commitments-host-report' }]],
  webServer: { command: 'npm run start:commitments-host -- --host 127.0.0.1 --port 4340', url: 'http://127.0.0.1:4340', reuseExistingServer: !process.env.CI, timeout: 120000 },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
