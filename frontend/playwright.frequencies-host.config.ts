import { defineConfig, devices } from '@playwright/test';
export default defineConfig({
  testDir: './projects/commitments-frequencies-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4330' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [['html', { outputFolder: 'playwright-frequencies-host-report' }]],
  webServer: { command: 'npm run start:frequencies-host -- --host 127.0.0.1 --port 4330', url: 'http://127.0.0.1:4330', reuseExistingServer: !process.env.CI, timeout: 120000 },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
