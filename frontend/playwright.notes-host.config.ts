import { defineConfig, devices } from '@playwright/test';
export default defineConfig({
  testDir: './projects/commitments-notes-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4360' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [['html', { outputFolder: 'playwright-notes-host-report' }]],
  webServer: { command: 'npm run start:notes-host -- --host 127.0.0.1 --port 4360', url: 'http://127.0.0.1:4360', reuseExistingServer: !process.env.CI, timeout: 120000 },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
