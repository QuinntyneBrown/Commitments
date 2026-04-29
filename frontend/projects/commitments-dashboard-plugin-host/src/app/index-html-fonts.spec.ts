import { readFileSync } from 'fs';
import { join } from 'path';

describe('commitments-dashboard-plugin-host index.html (font loading)', () => {
  const html = readFileSync(
    join(__dirname, '..', 'index.html'),
    'utf8'
  );

  it('preconnects to fonts.googleapis.com (bug-190)', () => {
    expect(html).toMatch(
      /<link\s+rel="preconnect"\s+href="https:\/\/fonts\.googleapis\.com"\s*>/
    );
  });

  it('preconnects to fonts.gstatic.com with crossorigin (bug-190)', () => {
    expect(html).toMatch(
      /<link\s+rel="preconnect"\s+href="https:\/\/fonts\.gstatic\.com"\s+crossorigin\s*>/
    );
  });

  it('Material Symbols Rounded font uses display=block, not swap (bug-190)', () => {
    const matchSwap = html.match(
      /family=Material\+Symbols\+Rounded[^"']*display=swap/
    );
    expect(matchSwap).toBeNull();

    const matchBlock = html.match(
      /family=Material\+Symbols\+Rounded[^"']*display=block/
    );
    expect(matchBlock).not.toBeNull();
  });

  it('Material Icons (legacy) font uses display=block (bug-190)', () => {
    const matchBlock = html.match(
      /family=Material\+Icons[^"']*display=block/
    );
    expect(matchBlock).not.toBeNull();
  });
});
