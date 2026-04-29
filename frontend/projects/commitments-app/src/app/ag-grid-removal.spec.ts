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

describe('grid package removal', () => {
  it('contains no removed grid imports anywhere in commitments-app', () => {
    const appSource = join(process.cwd(), 'projects/commitments-app/src/app');
    const removedPackagePrefix = 'ag' + '-grid';
    const removedImportPattern = new RegExp(
      `from ['"]${removedPackagePrefix}-(angular|community)['"]`,
    );
    const offenders = collectTypeScriptFiles(appSource).filter((file) =>
      removedImportPattern.test(readFileSync(file, 'utf8')),
    );

    expect(offenders).toEqual([]);
  });
});
