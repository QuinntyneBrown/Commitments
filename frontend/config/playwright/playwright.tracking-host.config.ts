import { defineConfig, devices } from '@playwright/test';
export default defineConfig({
  testDir: '../../projects/commitments-tracking-feature-host/e2e',
  use: { baseURL: 'http://127.0.0.1:4350' },
  workers: 1,
  retries: process.env.CI ? 2 : 0,
  reporter: [['html', { outputFolder: '../../playwright-tracking-host-report' }]],
  webServer: {
    command: 'npm run start:tracking-host -- --host 127.0.0.1 --port 4350',
    cwd: '../..',
    url: 'http://127.0.0.1:4350',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
});
