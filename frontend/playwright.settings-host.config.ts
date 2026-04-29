import { defineConfig, devices } from '@playwright/test';
export default defineConfig({
  testDir: './projects/commitments-settings-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4380' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [['html', { outputFolder: 'playwright-settings-host-report' }]],
  webServer: { command: 'npm run start:settings-host -- --host 127.0.0.1 --port 4380', url: 'http://127.0.0.1:4380', reuseExistingServer: !process.env.CI, timeout: 120000 },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
