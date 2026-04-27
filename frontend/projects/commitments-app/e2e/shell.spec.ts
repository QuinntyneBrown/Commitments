import { expect, test } from '@playwright/test';
import { ShellPage } from './pages/shell.page';

test.describe('application shell', () => {
  let shell: ShellPage;

  test.beforeEach(async ({ page }) => {
    shell = new ShellPage(page);
    await shell.goto();
  });

  test('sidenav nav-item icons match the design', async () => {
    const expected: Record<string, string> = {
      'Dashboard': 'dashboard',
      'Activities': 'directions_run',
      'Behaviours': 'psychology',
      'Behaviour Types': 'category',
      'Commitments': 'task_alt',
      'Cards': 'style',
      'Card Layouts': 'dashboard_customize',
      'Frequencies': 'schedule',
      'Notes': 'sticky_note_2',
      'Profiles': 'group',
      "To Do's": 'checklist',
      'Logout': 'logout',
    };

    for (const [label, icon] of Object.entries(expected)) {
      const iconEl = shell.navItemIcon(label);
      await expect(iconEl).toHaveText(icon);
    }
  });
});
