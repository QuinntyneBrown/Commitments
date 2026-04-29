import { test as setup } from '@playwright/test';
import * as fs from 'fs';

const authFile = 'playwright/.auth/user.json';

setup('authenticate', async ({ request }) => {
  const response = await request.post('https://localhost:63713/api/v1.0/users/token', {
    data: { username: 'quinntynebrown@gmail.com', password: 'P@ssw0rd' },
    ignoreHTTPSErrors: true,
  });

  const { accessToken } = await response.json();

  fs.mkdirSync('playwright/.auth', { recursive: true });

  fs.writeFileSync(authFile, JSON.stringify({
    cookies: [],
    origins: [
      {
        origin: 'http://127.0.0.1:4200',
        localStorage: [{ name: 'accessTokenKey', value: accessToken }],
      },
    ],
  }));
});
