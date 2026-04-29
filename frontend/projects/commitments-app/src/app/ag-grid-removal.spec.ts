import { readdirSync, readFileSync } from 'fs';
import { join } from 'path';

function collectTypeScriptFiles(directory: string): string[] {
  return readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const path = join(directory, entry.name);

    if (entry.isDirectory()) {
      return collectTypeScriptFiles(path);
    }

    return entry.isFile() && path.endsWith('.ts') ? [path] : [];
  });
}

describe('ag-grid removal', () => {
  it('contains no ag-grid imports anywhere in commitments-app', () => {
    const appSource = join(process.cwd(), 'projects/commitments-app/src/app');
    const offenders = collectTypeScriptFiles(appSource).filter((file) =>
      /from ['"]ag-grid-(angular|community)['"]/.test(readFileSync(file, 'utf8')),
    );

    expect(offenders).toEqual([]);
  });
});
