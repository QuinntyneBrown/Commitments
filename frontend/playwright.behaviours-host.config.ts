import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './projects/commitments-behaviours-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4320' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [['html', { outputFolder: 'playwright-behaviours-host-report' }]],
  webServer: {
    command: 'npm run start:behaviours-host -- --host 127.0.0.1 --port 4320',
    url: 'http://127.0.0.1:4320',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
